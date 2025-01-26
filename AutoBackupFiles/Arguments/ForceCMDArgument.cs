namespace AutoBackupFiles.Arguments;

public class ForceCMDArgument : GenericArgument
{
    public override string GetDescription() => "Forces the console to use special characters or not.";

    public override string GetArg() => "force-special-chars";

    public override void Execute(string[] split)
    {
        Console.ForceCmdMode(!bool.Parse(split[1]));
    }
}