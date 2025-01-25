namespace AutoBackupFiles;

public class Configuration
{
    public string DateFormat = "dd-mm-yyyy_HH-mm-ss";
    public Dictionary<string, OutputConfiguration> ToOutputs = new();
    public Dictionary<string, ElementToBackup> ToBackups = new();
    public Dictionary<string, ZIPConfiguration> ToZip = new();
    public FTPConfiguration[] ToFTP;
    public SSHConfiguration[] ToSSH;

    public void Finish()
    {
        foreach (var output in ToOutputs)
            output.Value.FixVars(DateTime.Now.ToString(DateFormat));
        foreach (var zip in ToZip)
            zip.Value.FixVars(DateTime.Now.ToString(DateFormat));
        ToFTP = ftpConfigurations.ToArray();
        ToSSH = sshConfigurations.ToArray();

        string date = DateTime.Now.ToString(DateFormat);
        foreach (var output in ToOutputs)
            output.Value.FixVars(date);
        foreach (var zip in ToZip)
            zip.Value.FixVars(date);
        foreach (var ftp in ftpConfigurations)
            ftp.FixVars(date);
        foreach (var ssh in sshConfigurations)
            ssh.FixVars(date);
    }
}