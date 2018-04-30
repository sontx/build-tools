using CommandLine;

namespace BuildTools.Ftp
{
    internal class ConfigOptions
    {
        [Option("config", HelpText = "Configuration file", Required = true)]
        public string ConfigFile { get; set; }
    }
}