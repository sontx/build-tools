using System;
using BuildTools.Utils;
using CommandLine;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Core;

namespace BuildTools.Zip
{
    internal class Program
    {
        private static ProgressBar _progressBar;
        private static int _uptoFileCount;
        private static int _totalFileCount;

        private static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<ZipOptions>(args);
            if (result.Errors.Any())
                return;

            var options = result.Value;
            var sourceFiles = SourceFile.Parse(options.SrcFile);

            if (File.Exists(options.DestFile))
                File.Delete(options.DestFile);

            var events = new FastZipEvents {ProcessFile = ProcessFileReport};
            var zip = new FastZip(events);

            Console.WriteLine("Computing...");
            var tempDir = PrepareDir(sourceFiles);
            _totalFileCount = ComputeFileCount(tempDir);

            Console.Write($"Zipping to {Path.GetFileName(options.DestFile)} ");
            try
            {
                _progressBar = new ProgressBar();
                zip.CreateZip(options.DestFile, tempDir, true, options.FileFilter, options.DirFilter);
            }
            finally
            {
                Directory.Delete(tempDir, true);
                _progressBar.Dispose();
            }

            Console.WriteLine("Done");
        }

        private static void ProcessFileReport(object sender, ScanEventArgs e)
        {
            _uptoFileCount++;
            var percentCompleted = _uptoFileCount * 100F / _totalFileCount;
            _progressBar.Report(percentCompleted);
        }

        private static string PrepareDir(string[] sourceFiles)
        {
            var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(dir);

            foreach (var sourceFile in sourceFiles)
            {
                if (sourceFile == null) continue;

                var dest = Path.Combine(dir, Path.GetFileName(sourceFile));
                if (Directory.Exists(sourceFile))
                {
                    dest = Path.Combine(dir, Path.GetFileName(sourceFile));
                    PathUtils.CopyDir(sourceFile, dest);
                }
                else
                {
                    File.Copy(sourceFile, dest);
                }
            }

            return dir;
        }

        private static int ComputeFileCount(string path)
        {
            var result = Directory.GetFiles(path).Length;
            var subFolders = Directory.GetDirectories(path);
            result += subFolders.Sum(ComputeFileCount);
            return result;
        }
    }
}