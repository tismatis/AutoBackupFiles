using System.IO.Compression;
using FluentFTP;

namespace AutoBackupFiles;

internal class Backup
{
    private static string _dateFormat = "dd-mm-yyyy_HH-mm-ss";
    private static string _destination = "";

    private static Dictionary<string, ObjectRestore>? _objects;
    
    public static bool ForceZip = false;
    public static bool ForceFtp = true;
    public static string FtpServer = "";
    public static string FtpUsername = "";
    public static string FtpPassword = "";
    public static string FtpPath = "";
    public static string FtpSecurity = "NoEncryption";
    
    public static void ListFiles(string[][] csv)
    {
        _objects = new Dictionary<string, ObjectRestore>();

        Console.Write("&7Starting listing...");
        for (var line = 0; line < csv.Length; line++)
        {
            string[] list = csv[line];
            switch (list[0])
            {
                case "folder":
                    if (!_objects.ContainsKey(list[1]))
                        _objects.Add(list[1], new ObjectRestore(list[1]));
                    _objects[list[1]].Paths.Add(["folder", list[2]]);
                    continue;
                case "file":
                    if (!_objects.ContainsKey(list[1]))
                        _objects.Add(list[1], new ObjectRestore(list[1]));
                    _objects[list[1]].Paths.Add(["file", list[2]]);
                    continue;
                case "ignore-folder":
                    if (!_objects.ContainsKey(list[1]))
                        throw new Exception(
                            "Listing error! An ignore-folder has been provided without being actually declared before!");
                    _objects[list[1]].Banned.Add(["folder", list[2]]);
                    break;
                case "ignore-file":
                    if (!_objects.ContainsKey(list[1]))
                        throw new Exception(
                            "Listing error! An ignore-file has been provided without being actually declared before!");
                    _objects[list[1]].Banned.Add(["file", list[2]]);
                    break;
                case "destination":
                    _destination = list[1];
                    continue;
                case "config":
                    switch (list[1])
                    {
                        case "date-format":
                            _dateFormat = list[2];
                            continue;
                        case "force-zip":
                            ForceZip = bool.Parse(list[2]);
                            continue;
                        case "ftp-server":
                            FtpServer = list[2];
                            continue;
                        case "ftp-username":
                            FtpUsername = list[2];
                            continue;
                        case "ftp-password":
                            FtpPassword = list[2];
                            continue;
                        case "ftp-path":
                            FtpPath = list[2];
                            continue;
                        case "ftp-security":
                            switch (list[2])
                            {
                                case "NoEncryption":
                                    FtpSecurity = list[2];
                                    continue;
                                case "Implicit":
                                    FtpSecurity = list[2];
                                    continue;
                                case "Explicit":
                                    FtpSecurity = list[2];
                                    continue; 
                                default:
                                    throw new Exception($"Parsing error! Unknown ftp-security option: {list[2]}");
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

        _destination = _destination.Replace("%DATE%", DateTime.Now.ToString(_dateFormat));
        
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("&aListing &lcomplete!            ");
        
        if(_destination == "")
            throw new Exception("No destination has been provided!");
    }

    public static void NativeBackup()
    {
        Console.Write("&7Starting backup...");

        foreach(KeyValuePair<string, ObjectRestore> obj in _objects)
        {
            Console.Write($"&7Copying {obj.Key}...");
            foreach(string[] path in obj.Value.Paths)
            {
                Console.Write($"&7Copying {path[1]}&r&7!");
                switch(path[0])
                {
                    case "folder":
                        CopyDirectory(path[1], Path.Combine(Path.Combine($"{_destination}", obj.Key), Path.GetFileName(path[1])), obj.Value.Banned);
                        break;
                    case "file":
                        CopyFile(path[1], Path.Combine(Path.Combine($"{_destination}", obj.Key), Path.GetFileName(path[1])), obj.Value.Banned);
                        break;
                    default:
                        throw new Exception("Parsing error! An non reconized line has been readed.");
                }
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&aCopied successfuly {path[1]}&r&a!");
            }
            Console.Write($"&7Writing list of backuped files for {obj.Key}...");
            File.WriteAllText(Path.Combine(Path.Combine($"{_destination}", obj.Key), "list_of_backuped_files.txt"), obj.Value.ToString());
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write($"&aCopied successfuly {obj.Key}&r&a!                                      ");
        }
        Console.Write("&aBackup done &lsuccessfully&r&a!");
    }

    public static void ZipBackup()
    {
        Console.Write("&7Starting backup...");
        Console.Write("&7Creating temporary directory...");
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        Console.Write("&aTemporary directory created!");
        Console.Write("&7Starting backup...");
        foreach(KeyValuePair<string, ObjectRestore> obj in _objects)
        {
            Console.Write($"&7Copying {obj.Key}...");
            foreach(string[] path in obj.Value.Paths)
            {
                Console.Write($"&7Copying {path[1]}&r&7!");
                switch(path[0])
                {
                    case "folder":
                        CopyDirectory(path[1], Path.Combine(Path.Combine($"{tempDir}", obj.Key), Path.GetFileName(path[1])), obj.Value.Banned);
                        break;
                    case "file":
                        CopyFile(path[1], Path.Combine(Path.Combine($"{tempDir}", obj.Key), Path.GetFileName(path[1])), obj.Value.Banned);
                        break;
                    default:
                        throw new Exception("Parsing error! An non reconized line has been readed.");
                }
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&aCopied successfuly {path[1]}&r&a!");
            }
            Console.Write($"&7Writing list of backuped files for {obj.Key}...");
            File.WriteAllText(Path.Combine(Path.Combine($"{tempDir}", obj.Key), "list_of_backuped_files.txt"), obj.Value.ToString());
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write($"&aCopied successfuly {obj.Key}&r&a!                                      ");
        }
        Console.Write("&aBackup done &lsuccessfully&r&a!");
        Console.Write("&7Creating zip file...");

        var files = Directory.GetFiles(tempDir, "*", SearchOption.AllDirectories);
        long totalSize = files.Sum(file => new FileInfo(file).Length);
        long processedSize = 0;

        using (var zip = ZipFile.Open($"{_destination}.zip", ZipArchiveMode.Create))
        {
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var entryName = Path.GetRelativePath(tempDir, file);
                zip.CreateEntryFromFile(file, entryName);
                processedSize += new FileInfo(file).Length;
                double progress = (double)processedSize / totalSize * 100;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"&eCreating zip file... {progress:0.00}% complete! [{(processedSize / 1024.0 / 1024.0):0.00}Mo processed, {(totalSize - processedSize) / 1024.0 / 1024.0:0.00}Mo remaining]", Program.ForceSaveDownloadLog);
            }
        }
        Console.Write("&aZip file created!");
        Console.Write("&7Deleting temporary directory...");
        Directory.Delete(tempDir, true);
        Console.Write("&aTemporary directory deleted!");
    }

    public static void FTPBackup()
    {
        Console.Write("&7Starting backup...");
        
        FtpEncryptionMode mode;
        switch (FtpSecurity)
        {
            case "NoEncryption":
                mode = FtpEncryptionMode.None;
                break;
            case "Implicit":
                mode = FtpEncryptionMode.Implicit;
                break;
            case "Explicit":
                mode = FtpEncryptionMode.Explicit;
                break;
            default:
                throw new Exception($"FTP Configuration Error! The encryption mode '{FtpSecurity}' is not supported.");
        }
        
        FTPSupport.UploadFileToFtp(mode, FtpServer, $"{_destination}.zip", $"{FtpPath}{Path.GetFileName($"{_destination}.zip")}", FtpUsername, FtpPassword);
        Console.Write("&aBackup done &lsuccessfully&r&a!");
    }
    
    private static void CopyDirectory(string sourceDir, string destDir, List<string[]> list)
    {
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            if(list.Any(l => l[0] == "file" && l[1] == file) && list.Any(l => l[0] == "folder" && file.StartsWith(l[1])))
                continue;
            
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            if(list.Any(l => l[0] == "folder" && directory.StartsWith(l[1])))
                continue;

            var destDirectory = Path.Combine(destDir, Path.GetFileName(directory));
            CopyDirectory(directory, destDirectory, list);
        }
    }

    private static void CopyFile(string sourceFile, string destFile, List<string[]> list)
    {
        var destDir = Path.GetDirectoryName(destFile);
        if (!Directory.Exists(destDir))
        {
            if (destDir != null) Directory.CreateDirectory(destDir);
        }

        if(list.Any(l => l[0] == "file" && l[1] == sourceFile) && list.Any(l => l[0] == "folder" && sourceFile.StartsWith(l[1])))
            return;
        
        File.Copy(sourceFile, destFile, true);
    }
}