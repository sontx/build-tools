using CommandLine;

namespace BuildTools.Ftp
{
    internal class FtpOptions
    {
        [Option("host", HelpText = "Host name or ip address", Required = true)]
        public string Host { get; set; }

        [Option("port", HelpText = "Ftp port", DefaultValue = 21, Required = false)]
        public int Port { get; set; }

        [Option("username", Required = false)]
        public string UserName { get; set; }

        [Option("password", Required = false)]
        public string Password { get; set; }

        [Option("dir", HelpText = "Upload directory")]
        public string UploadDir { get; set; }

        [Option("src", HelpText = "Source file", Required = true)]
        public string SrcFile { get; set; }

        [Option("dest", HelpText = "Destination file, default is the same as source file", Required = false)]
        public string DestFile { get; set; }
    }
}