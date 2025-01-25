namespace AutoBackupFiles.Config.Parser.Keys.OutputParsers;

public class FtpOutputParserKey : GenericOutputParserKey
{
    public override string GetName() => "ftp";

    public override void OnParseCSV(ref Configuration configuration, string[] line)
    {
        switch (line[3])
        {
            case "host":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToFTP[line[2]].Host = line[4];
                break;
            case "encryption":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToFTP[line[2]].SetEncryption(line[4]);
                break;
            case "user":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToFTP[line[2]].User = line[4];
                break;
            case "password":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToFTP[line[2]].Password = line[4];
                break;
            case "path":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToFTP[line[2]].Path = line[4];
                break;
            case "filename":
                if(!configuration.ToZip.ContainsKey(line[2]))
                    configuration.ToZip.Add(line[2], new ZIPConfiguration(line[2]));
                configuration.ToFTP[line[2]].FileName = line[4];
                break;
            default:
                throw new Exception($"FTP Configuration Parsing! An unknown settings has been given '{line[3]}'.");
        }
    }
}