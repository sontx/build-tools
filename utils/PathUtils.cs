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
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(src);

            if (!dir.Exists)
                throw new DirectoryNotFoundException(  $"Source directory does not exist or could not be found: {src}");

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(dest, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(dest, subdir.Name);
                CopyDir(subdir.FullName, temppath);
            }
        }
    }
}