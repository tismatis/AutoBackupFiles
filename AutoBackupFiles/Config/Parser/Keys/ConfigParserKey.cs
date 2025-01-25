using AutoBackupFiles.Keys;

namespace AutoBackupFiles.Config.Parser.Keys;

public class ConfigParserKey : GenericParserKey
{
    public override string GetName() => "config";

    public override void OnParseCSV(ref Configuration configuration, string[] line)
    {
        switch (line[1])
        {
            case "date-format":
                configuration.DateFormat = line[2];
                break;
            default:
                throw new Exception($"Configuration Parsing! An unknown settings has been given '{line[1]}'.");
        }
    }
}