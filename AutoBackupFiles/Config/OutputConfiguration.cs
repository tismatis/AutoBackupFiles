namespace AutoBackupFiles;

public class OutputConfiguration
{
    public string FolderName = "%DATE%";
    public string Path;

    public OutputConfiguration(string path, string folderName)
    {
        FolderName = folderName;
        Path = path;
    }
}