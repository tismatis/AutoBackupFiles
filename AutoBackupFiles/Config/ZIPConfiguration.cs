namespace AutoBackupFiles;

public class ZIPConfiguration
{
    public string Name;
    public string FileName = "%DATE%.ZIP";
    public string Path;

    public ZIPConfiguration(string name)
    {
        Name = name;
    }
    
    public void FixVars(string date)
    {
        FileName = FileName.Replace("%DATE%", date);
        Path = Path.Replace("%DATE%", date);
    }
}