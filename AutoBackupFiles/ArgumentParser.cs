using System.Reflection;
using AutoBackupFiles.Arguments;

namespace AutoBackupFiles;

public static class ArgumentParser
{
    public static Dictionary<string, GenericArgument> Keys = new();
    private static void SearchArguments()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        var derivedTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(GenericArgument).IsAssignableFrom(t));

        foreach(var type in derivedTypes)
        {
            var instance = (GenericArgument)Activator.CreateInstance(type)!;
            Keys.Add(instance.GetArg(), instance);
        }
    }
    
    public static void Parse(string[] args)
    {
        SearchArguments();
        
        foreach(var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                try
                {
                    string[] split = arg.Substring(2).Split("=");
                    Keys[split[0]].Execute(split);
                }
                catch (KeyNotFoundException)
                {
                    throw new Exception($"&4Invalid argument provided! '{arg.Substring(2)}'&r");
                }
                catch(Exception ex)
                {
                    throw new Exception("An error occured during process of argument.", ex);
                }
            }
            else
            {
                throw new Exception("Invalid argument provided! Please use '--' before argument.");
            }
        }
    }
}