using System.IO.Compression;

namespace AutoBackupFiles;

internal class Backup
{
    private static string _dateFormat = "dd-mm-yyyy_HH-mm-ss";
    private static string _destination = "";

    private static Dictionary<string, ObjectRestore> _objects;
    
    public static bool ForceZip = false;
    
    public static void ListFiles(string[][] csv)
    {
        _objects = new Dictionary<string, ObjectRestore>();

        Console.Write("&7Starting listing...");
        foreach (string[] list in csv)
        {
            switch(list[0])
            {
                case "folder":
                    if(!_objects.ContainsKey(list[1]))
                        _objects.Add(list[1], new ObjectRestore(list[1]));
                    _objects[list[1]].Paths.Add(["folder", list[2]]);
                    continue;
                case "file":
                    if(!_objects.ContainsKey(list[1]))
                        _objects.Add(list[1], new ObjectRestore(list[1]));
                    _objects[list[1]].Paths.Add([ "file", list[2] ]);
                    continue;
                case "destination":
                    _destination = list[1];
                    continue;
                case "config":
                    switch(list[1])
                    {
                        case "date-format":
                            _dateFormat = list[2];
                            continue;
                        case "force-zip":
                            ForceZip = bool.Parse(list[2]);
                            continue;
                        default:
                            throw new Exception($"Parsing error! An non reconized line has been readed. '{list[1]}'");
                    }
                default:
                    throw new Exception($"Parsing error! An non reconized line has been readed. '{list[0]}'");
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
                        CopyDirectory(path[1], Path.Combine(Path.Combine($"{_destination}", obj.Key), Path.GetFileName(path[1])));
                        break;
                    case "file":
                        CopyFile(path[1], Path.Combine(Path.Combine($"{_destination}", obj.Key), Path.GetFileName(path[1])));
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