using System;
using System.Collections.Generic;
using System.IO;
using AutoBackupFiles.BackupMethods;

namespace AutoBackupFiles;

internal static class Program
{
    public static bool ForceSaveDownloadLog = false;
    public static async Task Main(string[] args)
    {
        if(File.Exists("logs.txt"))
            File.Delete("logs.txt");
            
        await File.WriteAllTextAsync("logs.txt", "");
            
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
                    case "--force-save-download-log":
                        ForceSaveDownloadLog = true;
                        Console.Write("&cForcing save download log!");
                        Console.Write("Really why you want save this?");
                        break;
                    default:
                        throw new Exception("&4Invalid argument provided!&r");
                }
                
            Console.Write(Console.CmdMode ? "&aCMD Compatibility Mode Enabled!" : "&aCMD Compatibility Mode Disabled!");
                
            if(!File.Exists(args[0]))
                throw new Exception("&4The file you provided &ldoes not exist&r!");

            Configuration cfg = CSVParser.GetConfiguration(args[0]);

            string tempDir = OutputTemp.Backup(cfg);

            foreach (var bCfg in cfg.ToOutputs)
                OutputFolder.Backup(tempDir, bCfg);
            
            foreach (var bCfg in cfg.ToZip)
                OutputZip.Backup(tempDir, bCfg);
            
            Console.Write("&7Deleting temporary directory...");
            Directory.Delete(tempDir, true);
            Console.Write("&aTemporary directory deleted!");
        }catch(Exception ex)
        {
            Console.Write($"&4&lOno, an error has occured: &r&4{ex.Message}\n{ex.StackTrace}");
        }

        Console.Write("&7Press any key to &4exit&r&7 or wait 5 seconds...");
        var task = Task.Run(() =>
        {
            for (int i = 0; i < 50; i++)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    return;
                }
                Task.Delay(100).Wait();
            }
        });
        await task;
    }
}