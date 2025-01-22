namespace AutoBackupFiles;

public class ZIPConfiguration
{
    public string Name = "%DATE%.ZIP";
    public string FileName;
    public string Path;

    public ZIPConfiguration(string name)
    {
        Name = name;
    }
}