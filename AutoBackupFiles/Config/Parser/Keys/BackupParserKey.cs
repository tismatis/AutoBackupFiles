using AutoBackupFiles.Keys;

namespace AutoBackupFiles;

public class BackupParserKey : GenericParserKey
{
    public override string GetName() => "backup";

    public override void OnParseCSV(ref Configuration configuration, string[] line)
    {
        switch (line[1])
        {
            case "folder":
                if (!configuration.ToBackups.ContainsKey(line[2]))
                    configuration.ToBackups.Add(line[2], new ElementToBackup(line[2]));
                configuration.ToBackups[line[2]].ToInclude.Add(["folder", line[3]]);
                break;
            case "file":
                if (!configuration.ToBackups.ContainsKey(line[2]))
                    configuration.ToBackups.Add(line[2], new ElementToBackup(line[2]));
                configuration.ToBackups[line[2]].ToInclude.Add(["file", line[3]]);
                break;
            case "ignore-folder":
                if (!configuration.ToBackups.ContainsKey(line[2]))
                    throw new Exception(
                        "Configuration Parsing! An ignore-folder has been provided without being actually declared before!");
                configuration.ToBackups[line[2]].ToExclude.Add(["folder", line[3]]);
                break;
            case "ignore-file":
                if (!configuration.ToBackups.ContainsKey(line[2]))
                    throw new Exception(
                        "Configuration Parsing! An ignore-file has been provided without being actually declared before!");
                configuration.ToBackups[line[2]].ToExclude.Add(["file", line[3]]);
                break;
            default:
                throw new Exception($"Configuration Parsing! An unknown settings has been given '{line[1]}'.");
        }
    }
}