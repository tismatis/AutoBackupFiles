﻿namespace AutoBackupFiles.BackupMethods;

public static class OutputTemp
{
    public static string Backup(Configuration cfg)
    {
        Console.Write("&7Creating an temp folder...");
        string uniqueIdentifier = "";
        GenerateRandomChars(16).ToList().ForEach(c => uniqueIdentifier += c);
        string destination = Path.Combine(Path.GetTempPath(), $"AutoBackupFiles/{uniqueIdentifier}/");
        if(Directory.Exists(destination))
            Directory.Delete(destination, true);
        Directory.CreateDirectory(destination);
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write($"&aThe temp folder is at: {destination}.");
        
        Console.Write("&7Starting backup into Temp Folder...");

        foreach(var obj in cfg.ToBackups)
        {
            Console.Write($"&7Copying {obj.Value.Name}...");
            foreach(string[] path in obj.Value.ToInclude)
            {
                Console.Write($"&7Copying {path[1]}&r&7!");
                switch(path[0])
                {
                    case "folder":
                        FileManagement.CopyDirectory(path[1], Path.Combine(Path.Combine($"{destination}", obj.Value.Name), Path.GetFileName(path[1])), obj.Value.ToExclude);
                        break;
                    case "file":
                        FileManagement.CopyFile(path[1], Path.Combine(Path.Combine($"{destination}", obj.Value.Name), Path.GetFileName(path[1])), obj.Value.ToExclude);
                        break;
                    default:
                        throw new Exception("Parsing error! An non reconized line has been readed.");
                }
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&aCopied successfuly {path[1]}&r&a!");
            }
            Console.Write($"&7Writing list of backuped files for {obj.Value.Name}...");
            File.WriteAllText(Path.Combine(Path.Combine($"{destination}", obj.Value.Name), "list_of_files.txt"), obj.ToString());
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write($"&aCopied successfuly {obj.Value.Name}&r&a!                                      ");
        }
        Console.Write("&aBackup done &lsuccessfully&r&a in Temp Folder!");
        
        return destination;
    }
    
    private static char[] GenerateRandomChars(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.";
        
        Random random = new Random();
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }
        return result;
    }
}