using System.IO;

namespace BuildTools.Zip
{
    internal static class SourceFile
    {
        public static string[] Parse(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return lines;
        }
    }
}