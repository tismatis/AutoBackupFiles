namespace AutoBackupFiles;

public class OutputConfiguration
{
    public string Name;
    public string FolderName = "%DATE%";
    public string Path;

    public OutputConfiguration(string name, string path, string folderName)
    {
        Name = name;
        FolderName = folderName;
        Path = path;
    }

    public void FixVars(string date)
    {
        FolderName = FolderName.Replace("%DATE%", date);
        Path = Path.Replace("%DATE%", date);
    }
}