using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoBackupFiles
{
    internal static class Console
    {
        private static bool _k;
#region CMD Compatibility Mode
        private static bool _cmdMode;

        public static void Initialize()
        {
            _cmdMode = (Environment.GetEnvironmentVariable("TERM_PROGRAM") ??
                        Environment.GetEnvironmentVariable("ComSpec") ??
                        "Unknown").Contains("cmd.exe");
        }
        public static void ForceCmdMode(bool force) => _cmdMode = force;
#endregion CMD Compatibility Mode
#region System.Console Wrappers
        public static void Write(string message, bool disableTimezone = false) => WriteFormattedMessage($"{(!disableTimezone ? $"{DateTime.Now} &d&l>>&r " : "")}{message}");
        public static System.ConsoleKeyInfo ReadKey() => System.Console.ReadKey();
        public static void SetCursorPosition(int left, int top) => System.Console.SetCursorPosition(left, top);
        public static int CursorTop => System.Console.CursorTop;
        public static int CursorLeft => System.Console.CursorLeft;
#endregion System.Console Wrappers
#region Formatting
        private static void WriteFormattedMessage(string input)
        {
            string total = "";

            var message = input.Replace("&", "§");
            var regex = new Regex(@"§.");
            var matches = regex.Matches(message);
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                System.Console.Write(_k ? GenerateCensoredShit(match.Index - lastIndex) : message.Substring(lastIndex, match.Index - lastIndex));
                total += message.Substring(lastIndex, match.Index - lastIndex);
                
                lastIndex = match.Index + match.Length;

                GetConsoleColor(match.Value[1]);
            }

            System.Console.WriteLine(message.Substring(lastIndex));
            System.Console.ResetColor();
            if(!_cmdMode)
                System.Console.Write("\x1b[22m\x1b[23m\x1b[29m\x1b[24m");
            _k = false;

            File.AppendAllText("logs.txt", total + message.Substring(lastIndex) + "\n");
        }

        private static string GenerateCensoredShit(int length)
        {
            var random = new Random();
            const string chars = "█";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void GetConsoleColor(char colorCode)
        {
            if(_cmdMode)
                switch (colorCode)
                {
                    case 'l': // Bold
                        return;
                    case 'i': // Italic
                        return;
                    case 's': // Strikethrough
                        return;
                    case 'u': // Underline
                        return;
                    case 'r': // Reset
                        System.Console.ResetColor();
                        _k = false;
                        return;
                    default:
                        break;
                }
            
            switch(colorCode)
            {
                case '0':
                    System.Console.ForegroundColor = ConsoleColor.Black;
                    return;
                case '1':
                    System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                    return;
                case '2':
                    System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                    return;
                case '3':
                    System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return;
                case '4':
                    System.Console.ForegroundColor = ConsoleColor.DarkRed;
                    return;
                case '5':
                    System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    return;
                case '6':
                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return;
                case '7':
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                case '8':
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    return;
                case '9':
                    System.Console.ForegroundColor = ConsoleColor.Blue;
                    return;
                case 'a':
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    return;
                case 'b':
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    return;
                case 'c':
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    return;
                case 'd':
                    System.Console.ForegroundColor = ConsoleColor.Magenta;
                    return;
                case 'e':
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    return;
                case 'f':
                    System.Console.ForegroundColor = ConsoleColor.White;
                    return;
                case 'l': // Bold
                    System.Console.Write("\x1b[1m");
                    break;
                case 'i': // Italic
                    System.Console.Write("\x1b[3m");
                    break;
                case 's': // Strikethrough
                    System.Console.Write("\x1b[9m");
                    break;
                case 'u': // Underline
                    System.Console.Write("\x1b[4m");
                    break;
                case 'r': // Reset
                    System.Console.ResetColor();
                    System.Console.Write("\x1b[22m\x1b[23m\x1b[29m\x1b[24m"); // Reset bold, italic, strikethrough, underline
                    _k = false;
                    break;
                case 'k':
                    _k = true;
                    break;
                default:
                    throw new Exception($"{colorCode} is not a valid code.");
            }
        }
#endregion Formatting
    }
}