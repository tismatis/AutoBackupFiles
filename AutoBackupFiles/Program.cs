﻿using System;
using System.Collections.Generic;
using System.IO;

namespace AutoBackupFiles;

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
                
            Backup.ListFiles(csv);

            Backup.NativeBackup();
        }catch(Exception ex)
        {
            Console.Write($"&4&lOno, an error has occured: &r&4{ex.Message}\n{ex.StackTrace}");
        }

        Console.Write("&7Press any key to &4exit&r&7...");
        Console.ReadKey();
    }
}