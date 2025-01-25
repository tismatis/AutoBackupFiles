using System.Collections.Generic;
using System.IO;

namespace AutoBackupFiles
{
    public class CSVParser : GenericParser
    {
        public static bool VerboseParsing;
        
        private string[][] Parse(string path)
        {
            string[] lines = File.ReadAllLines(path);
            List<string[]> parsedLines = new List<string[]>();
            foreach (var line in lines)
            {
                if (line.StartsWith("#") || line == "" || line.StartsWith(" "))
                {
                    if (VerboseParsing)
                        Console.Write($"&7Skipping line: {line} (Empty or Comment)");
                    continue;
                }

                var split = line.Split(';');
                if (split[0] == "")
                {
                    if (VerboseParsing)
                        Console.Write($"&7Skipping line: {line} (Empty or Comment)");
                    continue;
                }
                parsedLines.Add(split);
            }

            return parsedLines.ToArray();
        }

        private Configuration GetConfiguration(string[][] csv)
        {
            Configuration cfg = new Configuration();
            
            for (var line = 0; line < csv.Length; line++)
            {
                string[] list = csv[line];
                try
                {
                    Keys[list[0]].OnParseCSV(ref cfg, list);
                }catch(KeyNotFoundException)
                {
                    Keys["obsolete"].OnParseCSV(ref cfg, list);
                }
                catch(Exception ex)
                {
                    throw new Exception($"&cError on line {line}: {ex.Message}", ex);
                }
            }
            
            cfg.Finish();
            
            return cfg;
        }

        public override Configuration GetConfiguration(string path)
        {
            Console.Write("&7Reading/Basic Parsing CSV file...");
            string[][] csv = Parse(path);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write("&aReading and Basic Parsing complete!");
            
            Console.Write("&7Starting listing...");
            var cfg =  GetConfiguration(csv);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write("&aListing complete!       ");
            
            return cfg;
        }
    }
}