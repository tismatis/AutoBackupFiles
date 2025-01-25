namespace AutoBackupFiles.Config.Parser.Keys.OutputParsers;

public class ZipOutputParserKey : GenericOutputParserKey
{
    public override string GetName() => "zip";

    public override void OnParseCSV(ref Configuration configuration, string[] line)
    {
        switch (line[3])
        {
            case "filename":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToZip[line[2]].FileName = line[4];
                break;
            case "path":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToZip[line[2]].Path = line[4];
                break;
            default:
                throw new Exception($"ZIP Configuration Parsing! An unknown settings has been given '{line[3]}'.");
        }
    }
}