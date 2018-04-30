using CommandLine;

namespace BuildTools.Zip
{
    internal class ZipOptions
    {
        [Option("src", HelpText = "Source file that contains file paths will be zipped", Required = true)]
        public string SrcFile { get; set; }

        [Option("dest", HelpText = "Destination file", Required = true)]
        public string DestFile { get; set; }

        [Option("dfilter", HelpText = "Filter directories, uses regular expressions which is tested against each directory name", Required = false)]
        public string DirFilter { get; set; }

        [Option("ffilter", HelpText = "Filter files that uses regular expressions. For Example, to only accept file extension is .txt, '@\\.txt$'", Required = false)]
        public string FileFilter { get; set; }
    }
}