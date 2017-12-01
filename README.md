# ComicArchive

A command line and library for manipulating comic archives (.cbz, .cbr, etc). Written in C# and .Net Core 2.0.

## Limitations

### Command Line App

### Comic Archive Library

The library currently only supports a basic conversion of an archive to a zip archive.

## Usage

### Command Line

```
   read|r: Read the metadata for a comic archive. Only supports reading Comic Rack metadata.
        /p /path : (String) (Required) (File exists)

   convert|c: Convert non-zip comic archives into zip archives.
        /o /overwrite     : If a file with the same name as the conversion target exists, replace it.
        /p /path          : The file or folder of files to convert. (String) (Required) (Path exists (file or directory))
        /r /replace       : Delete the original file once the conversion is complete.
        /s /showfullpaths : Show full file paths instead of just the files name.
```

### C# Library

TODO:

## To Do

- Implement parsing of xml metadata into a more readable format for presentation on the command line.
- Implement parsing of basic metadata from from a filename if metadata not present.
- [Eventually] Implement writing of metadata to archives. Only zip files will be supported for writing.
