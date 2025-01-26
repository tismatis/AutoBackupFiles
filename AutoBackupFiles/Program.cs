using AutoBackupFiles.BackupMethods;

namespace AutoBackupFiles;

internal static class Program
{
    public static bool ForceSaveDownloadLog = false;
    public static string PathConfigFile = "";
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
            
            ArgumentParser.Parse(args);
            if(PathConfigFile == "")
                throw new Exception("No path provided!");
                
            Console.Write(Console.CmdMode ? "&aCMD Compatibility Mode Enabled!" : "&aCMD Compatibility Mode Disabled!");
            
            GenericParser parser = new CSVParser();
            Configuration cfg = parser.GetConfiguration(PathConfigFile);

            string tempDir = OutputTemp.Backup(cfg);

            foreach (var bCfg in cfg.ToOutputs)
                OutputFolder.Backup(tempDir, bCfg.Value);
            
            foreach (var bCfg in cfg.ToZip)
                OutputZip.Backup(tempDir, bCfg.Value);

            foreach(var bCfg in cfg.ToFTP)
                OutputFTP.Backup(tempDir, bCfg.Value);
            
            foreach(var bCfg in cfg.ToSSH)
                OutputSSH.Backup(tempDir, bCfg.Value);
            
            Console.Write("&7Deleting temporary directory...");
            Directory.Delete(tempDir, true);
            Console.Write("&aTemporary directory deleted!");
        }catch(Exception ex)
        {
            Console.Write($"&4&lOno, an error has occured: &r&4{ex.Message}\n{ex.StackTrace}");
            
            
            Console.Write("&7Deleting temporary directory...");
            Directory.Delete(Path.Combine(Path.GetTempPath(), "AutoBackupFiles/"), true);
            Console.Write("&aTemporary directory deleted!");
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