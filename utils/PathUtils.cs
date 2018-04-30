using System.IO;
using System.Linq;

namespace BuildTools.Utils
{
    public static class PathUtils
    {
        public static string CombineUnix(params string[] paths)
        {
            var filtered = paths.Where(path => !string.IsNullOrEmpty(path));
            var combined = Path.Combine(filtered.ToArray());
            return combined.Replace("\\", "/");
        }

        public static void CopyDir(string src, string dest)
        {
            //Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(src, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(src, dest));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(src, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(src, dest), true);
            }
        }
    }
}