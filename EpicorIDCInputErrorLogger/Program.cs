using System;
using System.Configuration;
using System.IO;

namespace EpicorIDCInputErrorLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve settings from the config file
            string rootDirectory = ConfigurationManager.AppSettings["rootDirectory"];
            string allowedSubdirsConfig = ConfigurationManager.AppSettings["allowedSubdirs"];

            // Split the allowedSubdirs string into an array and trim whitespace
            string[] allowedSubdirs = allowedSubdirsConfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < allowedSubdirs.Length; i++)
            {
                allowedSubdirs[i] = allowedSubdirs[i].Trim();
            }

            // Process each allowed subdirectory
            foreach (var subdir in allowedSubdirs)
            {
                string errorDirPath = Path.Combine(rootDirectory, subdir, "Error files");

                if (Directory.Exists(errorDirPath))
                {
                    int fileCount = Directory.GetFiles(errorDirPath).Length;
                    if (fileCount > 0)
                    {
                        Console.WriteLine($"Subdirectory: {subdir}, Error File Count: {fileCount}");
                    }
                }
            }

            // Define the log file path with the current date in the filename
            string logDirectory = Path.Combine(rootDirectory, "0-ERRORS");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            string logFile = Path.Combine(logDirectory, $"error_log_{DateTime.Now:yyyyMMdd}.txt");

            // Create or overwrite the log file with a simple log message.
            File.WriteAllText(logFile, "Log generated on " + DateTime.Now);

            Console.WriteLine("Processing complete. Log file created at: " + logFile);
        }
    }
}
