using System.Reflection;
using AutoBackupFiles.Keys;

namespace AutoBackupFiles;

public class GenericParser
{
    public static Dictionary<string, GenericParserKey> Keys = new();
    
    public GenericParser()
    {
        Console.Write("Loading Keys for parsing...");
        Assembly assembly = Assembly.GetExecutingAssembly();

        var derivedTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(GenericParserKey).IsAssignableFrom(t));

        foreach(var type in derivedTypes)
        {
            var instance = (GenericParserKey)Activator.CreateInstance(type)!;
            Keys.Add(instance.GetName(), instance);
        }

        foreach (var kvp in Keys)
        {
            Console.Write($"{kvp.Key} = {Keys[kvp.Key].GetName()}");
        }
        
        Console.Write($"Loaded {Keys.Count} Keys!                              ");
    }
    
    public virtual Configuration GetConfiguration(string path)
    {
        return new Configuration();
    }
}