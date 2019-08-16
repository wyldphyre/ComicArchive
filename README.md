# ComicArchive

A command line and library for manipulating comic archives (.cbz, .cbr, etc). Written in C# and .Net Core 2.0.

## Limitations

### Command Line App

The `read` command only looks for embedded Comic Rack metadata. If that is not present it doesn't yet attempt to parse any information from the name of the file.

### Comic Archive Library

Things the library currently supports:

- Conversion of various archive formats to a zip archive.
- Extracting a file from an archive by name, based on a file path.
- Determining if a file path is an archive.

## Usage

### Command Line

```
   read|r: Read the metadata for a comic archive. Only supports reading Comic Rack metadata.
        /p /path : (String) (Required)

   convert|c: Convert non-zip comic archives into zip archives.
        /o /overwrite     : If a file with the same name as the conversion target exists, replace it.
        /p /path          : The file or folder of files to convert. (String) (Required) (Path exists (file or directory))
        /r /replace       : Delete the original file once the conversion is complete.
        /s /showfullpaths : Show full file paths instead of just the files name.
```

### C# Library

TODO:

## To Do List

### Short Term

- Move the conversion of an archive inside the ComicArchiveFile abstraction.
- Implement parsing of basic metadata from a filename if metadata not present.

### Long Term

- [Eventually] Implement writing of metadata to archives. Only zip files will be supported for writing initially. Other open standard formats may be supported one day, but Zip is the only open commonly used format, so there is probably no demand to support other formats.
