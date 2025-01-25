using AutoBackupFiles.Keys;

namespace AutoBackupFiles.Config.Parser.Keys;

public class ObsoleteParserKey : GenericParserKey
{
    public override string GetName() => "obsolete";

    public override void OnParseCSV(ref Configuration configuration, string[] line)
    {
        var skip = line[0] == "obsolete" ? 1 : 0;
        switch (line[skip])
        {
            case "folder":
                if (!configuration.ToBackups.ContainsKey(line[skip+1]))
                    configuration.ToBackups.Add(line[skip+1], new ElementToBackup(line[skip+1]));
                configuration.ToBackups[line[skip+1]].ToInclude.Add(["folder", line[skip+2]]);
                break;
            case "file":
                if (!configuration.ToBackups.ContainsKey(line[skip+1]))
                    configuration.ToBackups.Add(line[skip+1], new ElementToBackup(line[skip+1]));
                configuration.ToBackups[line[skip+1]].ToInclude.Add(["file", line[skip+2]]);
                break;
            case "ignore-folder":
                if (!configuration.ToBackups.ContainsKey(line[skip+1]))
                    throw new Exception(
                        "Configuration Parsing! An ignore-folder has been provided without being actually declared before!");
                configuration.ToBackups[line[skip+1]].ToExclude.Add(["folder", line[skip+2]]);
                break;
            case "ignore-file":
                if (!configuration.ToBackups.ContainsKey(line[skip+1]))
                    throw new Exception(
                        "Configuration Parsing! An ignore-file has been provided without being actually declared before!");
                configuration.ToBackups[line[skip+1]].ToExclude.Add(["file", line[skip+2]]);
                break;
            case "destination":
                configuration.ToOutputs.Add(line[skip+1], new OutputConfiguration("Default Output", line[1], ""));
                break;
        }
    }
}