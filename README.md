# ComicArchive

A command line and library for manipulating comic archives (.cbz, .cbr, etc). Written in C# and .Net Core 2.0.

## Limitations

### Command Line App

The `read` command currently just dumps the XML metadata, if it is present in the archive. Better presentation is coming next.

### Comic Archive Library

Things the library currently supports:

- Conversion of various archive formats to a zip archive.
- Extracting a file from an archive by name, based on a file path.
- Determining if a file path is an archive.

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
