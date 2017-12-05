# Generating the Comic Rack Classes from the XSD

Use the following steps to (re)generate the `ComicInfo.cs` file to produce the classes for working with the Comic Rack metadata structure.

## Windows

- Open the Visual Studio command prompt
- Navigate to the folder containing the `ComicInfo.xsd` file

Execute the following command.

```
>xsd ComicInfo.xsd /classes
```

This will create (or replace) the file `ComicInfo.cs`.