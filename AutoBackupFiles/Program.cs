using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoBackupFiles
{
    internal class Program
    {
        private static bool _cmdMode;
        public static void Main(string[] args)
        {
            if(File.Exists("logs.txt"))
                File.Delete("logs.txt");
            
            File.WriteAllText("logs.txt", "");
            
            string terminalName = Environment.GetEnvironmentVariable("TERM_PROGRAM") ?? 
                                  Environment.GetEnvironmentVariable("ComSpec") ?? 
                                  "Unknown";
            _cmdMode = terminalName.Contains("cmd.exe");
            
            try
            {
                WriteConsole("&5&l" + (@"
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
                            _cmdMode = false;
                            WriteConsole("&aForcing special chars!");
                            break;
                        case "--force-normal-chars":
                            _cmdMode = true;
                            WriteConsole("&cForcing ban special chars!");
                            break;
                        default:
                            throw new Exception("&4Invalid argument provided!&r");
                    }
                
                if(!File.Exists(args[0]))
                    throw new Exception("&4The file you provided &ldoes not exist&r!");

                WriteConsole("&7Reading/Basic Parsing CSV file...");
                
                string[][] csv = CSVParser.Parse(args[0]);
                
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                WriteConsole("&aReading and Basic Parsing complete!");
                
                string dateformat = "dd-mm-yyyy_HH-mm-ss";
                string destination = "";
                Dictionary<string, ObjectRestore> objects = new Dictionary<string, ObjectRestore>();

                WriteConsole("&7Starting listing...");
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
                WriteConsole("&aListing &lcomplete!            ");

                if(destination == "")
                    throw new Exception("No destination has been provided!");

                WriteConsole("&7Starting backup...");

                foreach(KeyValuePair<string, ObjectRestore> obj in objects)
                {
                    WriteConsole($"&7Copying {obj.Key}...");
                    foreach(string[] path in obj.Value.Paths)
                    {
                        WriteConsole($"&7Copying {path[1]}&r&7!");
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
                        WriteConsole($"&aCopied successfuly {path[1]}&r&a!");
                    }
                    WriteConsole($"&7Writing list of backuped files for {obj.Key}...");
                    File.WriteAllText(Path.Combine(Path.Combine($"{destination}", obj.Key), "list_of_backuped_files.txt"), obj.Value.ToString());
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    WriteConsole($"&aCopied successfuly {obj.Key}&r&a!                                      ");
                }
                WriteConsole("&aBackup done &lsuccessfully&r&a!");
            }catch(Exception ex)
            {
                WriteConsole($"&4&lOno, an error has occured: &r&4{ex.Message}\n{ex.StackTrace}");
            }

            WriteConsole("&7Press any key to &4exit&r&7...");
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
        
        private static void WriteConsole(string message, bool disableTimezone = false) => WriteConsoleSpecial($"{(!disableTimezone ? $"{DateTime.Now} &d&l>>&r " : "")}{message}");

        private static bool _k = false;
        private static void WriteConsoleSpecial(string input)
        {
            string total = "";

            var message = input.Replace("&", "§");
            var regex = new Regex(@"§.");
            var matches = regex.Matches(message);
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // Print the text before the color code
                Console.Write(_k ? GenerateCensoredShit(match.Index - lastIndex) : message.Substring(lastIndex, match.Index - lastIndex));
                total += message.Substring(lastIndex, match.Index - lastIndex);
                
                lastIndex = match.Index + match.Length;

                // Change the console color based on the color code
                GetConsoleColor(match.Value[1]);
            }

            // Print the remaining part of the message
            Console.WriteLine(message.Substring(lastIndex));
            Console.ResetColor();
            if(!_cmdMode)
                Console.Write("\x1b[22m\x1b[23m\x1b[29m\x1b[24m");
            _k = false;

            File.AppendAllText("logs.txt", total + message.Substring(lastIndex) + "\n");
        }

        private static string GenerateCensoredShit(int length)
        {
            var random = new Random();
            const string chars = "█";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void GetConsoleColor(char colorCode)
        {
            if(_cmdMode)
                switch (colorCode)
                {
                    case 'l': // Bold
                        return;
                    case 'i': // Italic
                        return;
                    case 's': // Strikethrough
                        return;
                    case 'u': // Underline
                        return;
                    case 'r': // Reset
                        Console.ResetColor();
                        _k = false;
                        return;
                    default:
                        break;
                }
            
            switch(colorCode)
            {
                case '0':
                    Console.ForegroundColor = ConsoleColor.Black;
                    return;
                case '1':
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    return;
                case '2':
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    return;
                case '3':
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return;
                case '4':
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    return;
                case '5':
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    return;
                case '6':
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return;
                case '7':
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                case '8':
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    return;
                case '9':
                    Console.ForegroundColor = ConsoleColor.Blue;
                    return;
                case 'a':
                    Console.ForegroundColor = ConsoleColor.Green;
                    return;
                case 'b':
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    return;
                case 'c':
                    Console.ForegroundColor = ConsoleColor.Red;
                    return;
                case 'd':
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    return;
                case 'e':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return;
                case 'f':
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                case 'l': // Bold
                    Console.Write("\x1b[1m");
                    break;
                case 'i': // Italic
                    Console.Write("\x1b[3m");
                    break;
                case 's': // Strikethrough
                    Console.Write("\x1b[9m");
                    break;
                case 'u': // Underline
                    Console.Write("\x1b[4m");
                    break;
                case 'r': // Reset
                    Console.ResetColor();
                    Console.Write("\x1b[22m\x1b[23m\x1b[29m\x1b[24m"); // Reset bold, italic, strikethrough, underline
                    _k = false;
                    break;
                case 'k':
                    _k = true;
                    break;
                default:
                    throw new Exception($"{colorCode} is not a valid code.");
            }
        }
    }
}