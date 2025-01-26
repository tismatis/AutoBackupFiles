namespace AutoBackupFiles.Arguments;

public class HelpArgument : GenericArgument
{
    public override string GetDescription() => "Shows the help menu.";

    public override string GetArg() => "help";

    public override void Execute(string[] split)
    {
        Console.Write("Help Menu:");
        foreach(var kvp in ArgumentParser.Keys)
        {
            Console.Write($"{kvp.Key} - {ArgumentParser.Keys[kvp.Key].GetDescription()}");
        }
        Environment.Exit(0);
    }
}