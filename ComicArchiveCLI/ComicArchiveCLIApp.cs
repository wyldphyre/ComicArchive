using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CLAP;
using CLAP.Validation;

using ComicArchive;

namespace ComicArchiveCLI
{
    class ComicArchiveCLIApp
    {
        [Verb(Description = "Read the metadata for a comic archive. Only supports reading Comic Rack (comicinfo.xml) metadata.")]
        public static void Read(
          [Required]
          [FileExists]
          string path
        )
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine($"{path} is a directory. Pass a file path instead.");
                return;
            }

            if (!ComicArchive.ArchiveHelper.IsArchive(path))
            {
                Console.WriteLine($"{path} is not an archive.");
                return;
            }

            try
            {
                var comic = new ComicArchive.File { Path = path };
                comic.ReadMetadataFromArchive();

                if (!comic.HasMetadataStream || comic.MetadataStream.Length == 0)
                {
                    // TODO: Extend the library to parse metadata from the file name, if possible
                    Console.WriteLine($"Comic Rack metadata file '{ArchiveHelper.comicRackMetadataFilename}' missing.");
                }
                else
                {
                    Console.WriteLine(comic.MetadataAsText());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Verb(Description = "Write metadata tags to comic archive. Only supports writing Comic Rack (comicinfo.xml) metadata.")]
        public static void Write(
            [Required]
            string path,
            [Required]
            string metadata,
            bool confirmDirectory
        )
        {
            var pathIsDirectory = Directory.Exists(path);

            if (pathIsDirectory && !confirmDirectory)
            {
                Console.WriteLine($"Process directory? (y/n): {path}");
                var response = Console.ReadLine()?.Trim();

                if (!response.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }

            if (!pathIsDirectory && !ComicArchive.ArchiveHelper.IsZipArchive(path))
            {
                Console.WriteLine($"{path} is not a zip archive.");
                return;
            }

            if (string.IsNullOrWhiteSpace(metadata) || !metadata.Contains('='))
            {
                Console.WriteLine("Invalid metadata");
                return;
            }

            try
            {
                var newMetadata = metadata.Split(';')
                    .Select(value =>value.Split('='))
                    .ToDictionary(kvp => kvp[0].ToLower(), kvp => kvp[1]);

                var files = !pathIsDirectory 
                    ? [path] 
                    : Directory.GetFiles(path, "*.cbz").Where(f => ComicArchive.ArchiveHelper.IsZipArchive(f));

                foreach (var file in files)
                {
                    var comic = new ComicArchive.File { Path = file };
                    comic.ReadMetadataFromArchive();

                    var filename = Path.GetFileName(file);
                    if (!comic.HasMetadataStream || comic.MetadataStream.Length == 0)
                    {
                        Console.Write($"{filename} - Creating Comic Rack metadata... ");
                    }
                    else
                    {
                        Console.Write($"{filename} - Updating Comic Rack metadata... ");
                    }

                    comic.UpdateMetadata(newMetadata);

                    comic.SaveMetadataToArchive();

                    ConsoleWriteLine("Done", withColor: ConsoleColor.Green);
                }

                Console.WriteLine("Finished");
            }
            catch (ArgumentException e)
            {
                if (e.Message.StartsWith("An item with the same key has already been added"))
                {
                    var problemAttribute = e.Message.Split("Key:")[1].Trim();
                    ConsoleWriteLine($"Duplicate metadata attribute: {problemAttribute}", withColor: ConsoleColor.Red);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Verb(Description = "Convert non-zip comic archives into zip archives.")]
        public static void Convert(
            [Required]
            [Description("The file or folder of files to convert.")]
            string path,
            [Description("If a file with the same name as the conversion target exists, replace it.")]
            bool overwrite,
            [Description("Delete the original file once the conversion is complete.")]
            bool replace,
            [Description("Show full file paths instead of just the files name.")]
            bool showFullPaths
        )
        {
            var options = new ComicArchive.ConverterOptions
            {
                OverwriteExisting = overwrite,
                ReplaceOriginalFile = replace
            };
            options.LogOptions.ShowFullPaths = showFullPaths;

            var converter = new ComicArchive.Converter();
            converter.LogActivityEvent += (message) => Console.WriteLine(message);

            if (path == ".")
            {
                path = Environment.CurrentDirectory;
            }

            var pathDirectory = Path.GetDirectoryName(path);
            var pathFileMask = Path.GetFileName(path);

            if (!Directory.Exists(pathDirectory))
            {
                Console.WriteLine($"Directory '{pathDirectory}' does not exist.");
                return;
            }

            var alertOnInvalidArchives = true;

            if (pathFileMask == string.Empty)
            {
                pathFileMask = "*.*";
                alertOnInvalidArchives = false;
            }

            var pathDirectoryInfo = new DirectoryInfo(pathDirectory);
            var filePathsToConvert = pathDirectoryInfo.GetFiles(pathFileMask)
              .Where(fi => !fi.Name.StartsWith('.'))
              .Select(fi => fi.FullName);

            foreach (var filePath in filePathsToConvert)
            {
                if (!ComicArchive.ArchiveHelper.IsArchive(filePath))
                {
                    if (alertOnInvalidArchives)
                    {
                        Console.Beep();
                    }

                    Console.WriteLine($"{filePath} is not a valid archive!");
                }
            }

            filePathsToConvert = filePathsToConvert.Where(filePath => ComicArchive.ArchiveHelper.IsArchive(filePath));

            if (!filePathsToConvert.Any())
            {
                Console.WriteLine($"Could not find any files matching '{pathFileMask}' to convert.");
                return;
            }

            try
            {
                foreach (var filePath in filePathsToConvert)
                {
                    var (result, message) = converter.ConvertToZipArchive(filePath, options, out var convertedPath);

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        Console.WriteLine(message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}\n\n");
                Console.WriteLine(e.StackTrace);
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("Conversion finished!");
            // TODO: output some stats on files skipped and files converted
        }

        [Empty, Help]
        public static void Help(string help)
        {
            Console.WriteLine(help);
        }

        [Error]
        public static void Error(CLAP.ExceptionContext context)
        {
            Console.WriteLine(context.Exception.Message);
            //Console.WriteLine(context.Exception.StackTrace);
        }

        private static void ConsoleWriteLine(string message, ConsoleColor withColor)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = withColor;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}