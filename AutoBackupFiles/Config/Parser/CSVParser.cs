using System.Collections.Generic;
using System.IO;

namespace AutoBackupFiles
{
    public class CSVParser
    {
        private static string[][] Parse(string path)
        {
            string[] lines = File.ReadAllLines(path);
            List<string[]> parsedLines = new List<string[]>();
            foreach (var line in lines)
            {
                if(line.StartsWith("#") || line == "" || line.StartsWith(" "))
                    continue;

                var split = line.Split(';');
                if (split[0] == "")
                    continue;
                parsedLines.Add(split);
            }

            return parsedLines.ToArray();
        }

        private static Configuration GetConfiguration(string[][] csv)
        {
            Dictionary<string, OutputConfiguration> outputConfigurations = new Dictionary<string, OutputConfiguration>();
            Dictionary<string, ZIPConfiguration> zipConfigurations = new Dictionary<string, ZIPConfiguration>();
            Dictionary<string, ElementToBackup> toBackups = new Dictionary<string, ElementToBackup>();
            
            Configuration cfg = new Configuration();
            
            for (var line = 0; line < csv.Length; line++)
            {
                string[] list = csv[line];
                switch (list[0])
                {
                    #region Obsolete
                    case "folder":
                        if (!toBackups.ContainsKey(list[1]))
                            toBackups.Add(list[1], new ElementToBackup(list[1]));
                        toBackups[list[1]].ToInclude.Add(["folder", list[2]]);
                        continue;
                    case "file":
                        if (!toBackups.ContainsKey(list[1]))
                            toBackups.Add(list[1], new ElementToBackup(list[1]));
                        toBackups[list[1]].ToInclude.Add(["file", list[2]]);
                        continue;
                    case "ignore-folder":
                        if (!toBackups.ContainsKey(list[1]))
                            throw new Exception(
                                "Configuration Parsing! An ignore-folder has been provided without being actually declared before!");
                        toBackups[list[1]].ToExclude.Add(["folder", list[2]]);
                        continue;
                    case "ignore-file":
                        if (!toBackups.ContainsKey(list[1]))
                            throw new Exception(
                                "Configuration Parsing! An ignore-file has been provided without being actually declared before!");
                        toBackups[list[1]].ToExclude.Add(["file", list[2]]);
                        continue;
                    case "destination":
                        outputConfigurations.Add(list[1], new OutputConfiguration("Default Output", list[1], ""));
                        continue;
                    #endregion Obsolete
                    case "backup":
                        switch (list[1])
                        {
                            case "folder":
                                if (!toBackups.ContainsKey(list[2]))
                                    toBackups.Add(list[2], new ElementToBackup(list[2]));
                                toBackups[list[2]].ToInclude.Add(["folder", list[3]]);
                                continue;
                            case "file":
                                if (!toBackups.ContainsKey(list[1]))
                                    toBackups.Add(list[2], new ElementToBackup(list[2]));
                                toBackups[list[2]].ToInclude.Add(["file", list[3]]);
                                continue;
                            case "ignore-folder":
                                if (!toBackups.ContainsKey(list[2]))
                                    throw new Exception(
                                        "Configuration Parsing! An ignore-folder has been provided without being actually declared before!");
                                toBackups[list[2]].ToExclude.Add(["folder", list[3]]);
                                continue;
                            case "ignore-file":
                                if (!toBackups.ContainsKey(list[2]))
                                    throw new Exception(
                                        "Configuration Parsing! An ignore-file has been provided without being actually declared before!");
                                toBackups[list[2]].ToExclude.Add(["file", list[3]]);
                                continue;
                            default:
                                throw new Exception($"Configuration Parsing! An unknown settings has been given '{list[1]}'.");
                        }
                    case "output":
                        outputConfigurations.Add(list[1], new OutputConfiguration(list[1], list[2], list.Length == 4 ? list[3] : ""));
                        continue;
                    case "config":
                        switch (list[1])
                        {
                            case "date-format":
                                cfg.DateFormat = list[2];
                                continue;
                            case "zip":
                                if(list.Length != 5)
                                    throw new Exception("The zip file format is invalid!");
                                
                                switch (list[3])
                                {
                                    case "path":
                                        if(!zipConfigurations.ContainsKey(list[2]))
                                            zipConfigurations.Add(list[2], new ZIPConfiguration(list[2]));
                                        zipConfigurations[list[2]].Path = list[3];
                                        continue;
                                    case "filename":
                                        if(!zipConfigurations.ContainsKey(list[2]))
                                            zipConfigurations.Add(list[2], new ZIPConfiguration(list[2]));
                                        zipConfigurations[list[2]].FileName = list[3];
                                        continue;
                                    default:
                                        throw new Exception("The zip file format is invalid!");
                                }
                            default:
                                throw new Exception(
                                    $"Parsing error! An non reconized line has been readed at line {line}, column 2. '{list[1]}'");
                        }
                    default:
                        throw new Exception(
                            $"Parsing error! An non reconized line has been readed at line {line}, column 1. '{list[0]}'");
                }
            }
            
            cfg.Finish(outputConfigurations.Values.ToList(), toBackups.Values.ToList(), zipConfigurations.Values.ToList());
            
            return cfg;
        }

        public static Configuration GetConfiguration(string path)
        {
            Console.Write("&7Reading/Basic Parsing CSV file...");
            string[][] csv = Parse(path);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write("&aReading and Basic Parsing complete!");
            
            Console.Write("&7Starting listing...");
            var cfg =  GetConfiguration(csv);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write("&aListing complete!       ");
            
            return cfg;
        }
    }
}