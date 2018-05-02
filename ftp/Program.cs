using BuildTools.Utils;
using CommandLine;
using FluentFTP;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace BuildTools.Ftp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParserResult<FtpOptions> ftpOptionsResult;

            if (args.Length == 1)
            {
                var configOptionsResult = Parser.Default.ParseArguments<ConfigOptions>(args);
                if (configOptionsResult.Errors.Any())
                    return;

                if (!VerifyConfigOptions(configOptionsResult)) return;

                var config = CommandLineUtils.CommandLineToArgs(File.ReadAllText(configOptionsResult.Value.ConfigFile));
                ftpOptionsResult = Parser.Default.ParseArguments<FtpOptions>(config);
                if (ftpOptionsResult.Errors.Any())
                    return;
            }
            else
            {
                ftpOptionsResult = Parser.Default.ParseArguments<FtpOptions>(args);
                if (ftpOptionsResult.Errors.Any())
                    return;
            }

            var options = ftpOptionsResult.Value;

            while (true)
            {
                try
                {
                    if (!VerifyFtpOptions(options)) return;
                    UploadToServer(options);
                }
                catch (FtpCommandException exception)
                {
                    if (exception.Message.Contains("Login"))
                    {
                        Console.WriteLine("Password is incorrect, try again!");
                        options.Password = null;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static bool VerifyConfigOptions(ParserResult<ConfigOptions> configOptionsResult)
        {
            var configOptions = configOptionsResult.Value;
            if (string.IsNullOrEmpty(configOptions.ConfigFile) || !File.Exists(configOptions.ConfigFile))
            {
                Console.WriteLine($"{configOptions.ConfigFile} is not found.");
                return false;
            }

            return true;
        }

        private static void UploadToServer(FtpOptions options)
        {
            using (var client = new FtpClient(options.Host))
            {
                client.Credentials = new NetworkCredential(options.UserName, options.Password);
                client.Connect();

                var srcFile = options.SrcFile;
                var destFileName = string.IsNullOrEmpty(options.DestFile)
                    ? Path.GetFileName(srcFile)
                    : options.DestFile;

                var workdingDir = client.GetWorkingDirectory();
                var destFilePath = PathUtils.CombineUnix(workdingDir, options.UploadDir, destFileName);

                Console.Write($"Uploading {Path.GetFileName(srcFile)} ");
                using (var progressBar = new ProgressBar())
                {
                    client.UploadFile(
                        srcFile,
                        destFilePath,
                        FtpExists.Overwrite,
                        true,
                        FtpVerify.Retry,
                        progressBar);
                }
                Console.WriteLine("\nDone");
            }
        }

        private static bool VerifyFtpOptions(FtpOptions options)
        {
            if (string.IsNullOrEmpty(options.SrcFile) || !File.Exists(options.SrcFile))
            {
                Console.WriteLine($"{options.SrcFile} is not found.");
                return false;
            }

            if (!string.IsNullOrEmpty(options.UserName) && string.IsNullOrEmpty(options.Password))
            {
                Console.Write("Password: ");
                options.Password = PasswordInput.GetPassword();
                Console.WriteLine();
            }

            return true;
        }
    }
}