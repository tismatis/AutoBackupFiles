namespace AutoBackupFiles;

public class Configuration
{
    public string DateFormat = "dd-mm-yyyy_HH-mm-ss";
    public OutputConfiguration[] ToOutputs;
    public ElementToBackup[] ToBackups;
    public ZIPConfiguration[] ToZip;
    public FTPConfiguration[] ToFTP;
    public SSHConfiguration[] ToSSH;

    public void Finish(List<OutputConfiguration> toOutputs, List<ElementToBackup> toBackups, List<ZIPConfiguration> zipConfigurations, List<FTPConfiguration> ftpConfigurations, List<SSHConfiguration> sshConfigurations)
    {
        ToOutputs = toOutputs.ToArray();
        ToZip = zipConfigurations.ToArray();
        ToBackups = toBackups.ToArray();
        ToFTP = ftpConfigurations.ToArray();
        ToSSH = sshConfigurations.ToArray();

        foreach (var output in toOutputs)
            output.FixVars(DateTime.Now.ToString(DateFormat));
        foreach (var zip in zipConfigurations)
            zip.FixVars(DateTime.Now.ToString(DateFormat));
        foreach (var ftp in ftpConfigurations)
        {
            ftp.Path = ftp.Path.Replace("%DATE%", DateTime.Now.ToString(DateFormat));
            ftp.Name = ftp.Path.Replace("%DATE%", DateTime.Now.ToString(DateFormat));
        }
        foreach (var ssh in sshConfigurations)
        {
            ssh.Path = ssh.Path.Replace("%DATE%", DateTime.Now.ToString(DateFormat));
            ssh.Name = ssh.Path.Replace("%DATE%", DateTime.Now.ToString(DateFormat));
        }
    }
}