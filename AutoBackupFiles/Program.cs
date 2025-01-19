using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoBackupFiles
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if(File.Exists("logs.txt"))
                File.Delete("logs.txt");
            
            File.WriteAllText("logs.txt", "");
            
            Console.Initialize();
            
            try
            {
                Console.Write("&5&l" + (@"
 ________  ________  ________  ___  __    ___  ___  ________   
|\   __  \|\   __  \|\   ____\|\  \|\  \ |\  \|\  \|\   __  \  
\ \  \|\ /\ \  \|\  \ \  \___|\ \  \/  /|\ \  \\\  \ \  \|\  \ 
 \ \   __  \ \   __  \ \  \    \ \   ___  \ \  \\\  \ \   ____\
  \ \  \|\  \ \  \ \  \ \  \____\ \  \\ \  \ \  \\\  \ \  \___|
   \ \_______\ \__\ \__\ \_______\ \__\\ \__\ \_______\ \__\   
    \|_______|\|__|\|__|\|_______|\|__| \|__|\|_______|\|__|   
=================================================================
" + $"Tismatis - Auto Backup Files - Version %VERSION%" + @"
=================================================================").Substring(1));
                if (args.Length == 0)
                    throw new Exception("&4Please provide a path to the &lfile you want to backup&r&4!");
                
                if(args.Length > 2)
                    throw new Exception("&4Too many arguments provided!&r");

                if(args.Length == 2)
                    switch (args[1])
                    {
                        case "--force-special-chars":
                            Console.ForceCmdMode(false);
                            Console.Write("&aForcing special chars!");
                            break;
                        case "--force-normal-chars":
                            Console.ForceCmdMode(true);
                            Console.Write("&cForcing ban special chars!");
                            break;
                        default:
                            throw new Exception("&4Invalid argument provided!&r");
                    }
                
                Console.Write(Console.CmdMode ? "&aCMD Compatibility Mode Enabled!" : "&aCMD Compatibility Mode Disabled!");
                
                if(!File.Exists(args[0]))
                    throw new Exception("&4The file you provided &ldoes not exist&r!");

                Console.Write("&7Reading/Basic Parsing CSV file...");
                
                string[][] csv = CSVParser.Parse(args[0]);
                
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("&aReading and Basic Parsing complete!");
                
                string dateformat = "dd-mm-yyyy_HH-mm-ss";
                string destination = "";
                Dictionary<string, ObjectRestore> objects = new Dictionary<string, ObjectRestore>();

                Console.Write("&7Starting listing...");
                foreach (string[] list in csv)
                {
                    if(list[0].StartsWith(" ") && list.Length == 1 || list[0] == "" || list[0].StartsWith("#") && list.Length == 1)
                        continue;

                    switch(list[0])
                    {
                        case "folder":
                            if(!objects.ContainsKey(list[1]))
                                objects.Add(list[1], new ObjectRestore(list[1]));
                            objects[list[1]].Paths.Add(new string[] { "folder", list[2] });
                            continue;
                        case "file":
                            if(!objects.ContainsKey(list[1]))
                                objects.Add(list[1], new ObjectRestore(list[1]));
                            objects[list[1]].Paths.Add(new string[] { "file", list[2] });
                            continue;
                        case "destination":
                            destination = list[1];
                            continue;
                        case "config":
                            switch(list[1])
                            {
                                case "date-format":
                                    dateformat = list[2];
                                    continue;
                                default:
                                    throw new Exception("Parsing error! An non reconized line has been readed.");
                            }
                        default:
                            throw new Exception("Parsing error! An non reconized line has been readed.");
                    }
                }

                destination = destination.Replace("%DATE%", DateTime.Now.ToString(dateformat));

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("&aListing &lcomplete!            ");

                if(destination == "")
                    throw new Exception("No destination has been provided!");

                Console.Write("&7Starting backup...");

                foreach(KeyValuePair<string, ObjectRestore> obj in objects)
                {
                    Console.Write($"&7Copying {obj.Key}...");
                    foreach(string[] path in obj.Value.Paths)
                    {
                        Console.Write($"&7Copying {path[1]}&r&7!");
                        switch(path[0])
                        {
                            case "folder":
                                CopyDirectory(path[1], Path.Combine(Path.Combine($"{destination}", obj.Key), Path.GetFileName(path[1])));
                                break;
                            case "file":
                                CopyFile(path[1], Path.Combine(Path.Combine($"{destination}", obj.Key), Path.GetFileName(path[1])));
                                break;
                            default:
                                throw new Exception("Parsing error! An non reconized line has been readed.");
                        }
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write($"&aCopied successfuly {path[1]}&r&a!");
                    }
                    Console.Write($"&7Writing list of backuped files for {obj.Key}...");
                    File.WriteAllText(Path.Combine(Path.Combine($"{destination}", obj.Key), "list_of_backuped_files.txt"), obj.Value.ToString());
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write($"&aCopied successfuly {obj.Key}&r&a!                                      ");
                }
                Console.Write("&aBackup done &lsuccessfully&r&a!");
            }catch(Exception ex)
            {
                Console.Write($"&4&lOno, an error has occured: &r&4{ex.Message}\n{ex.StackTrace}");
            }

            Console.Write("&7Press any key to &4exit&r&7...");
            Console.ReadKey();
        }
        
        private static void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                var destDirectory = Path.Combine(destDir, Path.GetFileName(directory));
                CopyDirectory(directory, destDirectory);
            }
        }

        private static void CopyFile(string sourceFile, string destFile)
        {
            var destDir = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            File.Copy(sourceFile, destFile, true);
        }
    }
}