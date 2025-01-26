namespace AutoBackupFiles.Arguments;

public class ForceSaveDownloadArgument : GenericArgument
{
    public override string GetDescription() => "Forces the Program to save downloads progress in log or not.";

    public override string GetArg() => "force-save-downloads";

    public override void Execute(string[] split)
    {
        Program.ForceSaveDownloadLog = !bool.Parse(split[1]);
    }
}