namespace AutoBackupFiles.Arguments;

public class PathArgument : GenericArgument
{
    public override string GetDescription() => "The path to the file .csv.";
    public override string GetArg() => "path";

    public override void Execute(string[] split)
    {
        Program.PathConfigFile = split[1];
    }
}