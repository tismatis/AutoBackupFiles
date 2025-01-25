using System.Reflection;
using AutoBackupFiles.Config.Parser.Keys.OutputParsers;
using AutoBackupFiles.Keys;

namespace AutoBackupFiles.Config.Parser.Keys;

public class OutputParserKey : GenericParserKey
{
    public GenericOutputParserKey[] Keys;
    
    public OutputParserKey()
    {
        if(Keys != null)
            return;

        Console.Write("Loading Output Keys for parsing...");
        Assembly assembly = Assembly.GetExecutingAssembly();

        var derivedTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(GenericOutputParserKey).IsAssignableFrom(t));

        Keys = derivedTypes.Select(t => (GenericOutputParserKey)Activator.CreateInstance(t)).ToArray();
        Console.SetCursorPosition(0, Console.CursorTop -1);
        Console.Write($"Loaded Output {Keys.Length} Keys !               ");
    }
    
    public override string GetName() => "output";

    public override void OnParseCSV(ref Configuration configuration, string[] line)
    {
        switch (line[1])
        {
            case "folder":
                configuration.ToOutputs.Add(line[2], new OutputConfiguration(line[2], line[3], line.Length == 5 ? line[4] : ""));
                break;
            default:
                var key = Keys.Single(k => k.GetName() == line[1]);
                if(key == null)
                    throw new Exception($"Output Configuration Parsing! An unknown settings has been given '{line[1]}'.");
                key.OnParseCSV(ref configuration, line);
                break;
        }
    }
}