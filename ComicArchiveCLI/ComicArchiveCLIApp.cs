﻿using System;
using System.IO;
using System.Linq;
using CLAP;
using CLAP.Validation;
using ComicArchive;

namespace ComicArchiveCLI
{
    class ComicArchiveCLIApp
    {
        [Verb(Description = "Read the metadata for a comic archive. Only supports reading Comic Rack metadata.")]
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
                    Console.WriteLine(comic.MetadataAsText());
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
                path = Environment.CurrentDirectory;

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
              .Where(fi => !fi.Name.StartsWith("."))
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
                        Console.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}\n\n");
                Console.WriteLine(e.StackTrace);
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("Conversion finshed!");
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
    }
}