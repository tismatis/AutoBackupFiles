namespace AutoBackupFiles.Arguments;

public abstract class GenericArgument
{
    public abstract string GetDescription();
    public abstract string GetArg();
    public abstract void Execute(string[] split);
}