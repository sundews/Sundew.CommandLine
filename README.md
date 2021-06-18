# Sundew.CommandLine

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/hugener/Sundew.CommandLine/.NET?label=GitHub%20Actions&logo=github)
![Nuget](https://img.shields.io/nuget/v/Sundew.CommandLine)

## Features
- *nix style command line. -x, --xx
- Includes command line parser, generator and help generator
- Supports verbs, arguments, switches (flags) and values (positional elements)
- Object oriented (Implement IVerb or IArguments)
- Parses/Generates: simple types, lists, nested types.
- Supports optional/required arguments/values
- Supports any of a given number of arguments
- Supports enums
- Nested arguments for argument grouping and reuse

## Samples
### Optional, optional list and switch
**Usage**  
https://github.com/sundews/Sundew.Packaging/blob/main/Source/Sundew.Packaging.Tool/Program.cs  
**Configure**  
https://github.com/sundews/Sundew.Packaging/blob/main/Source/Sundew.Packaging.Tool/UpdateVerb.cs

### Required value
**Configure**  
https://github.com/sundews/Sundew.Packaging/blob/main/Source/Sundew.Packaging.Tool/AwaitPublishVerb.cs

### Required list, require any of and enum
**Usage**  
https://github.com/sundews/CommandLineBatcher/blob/main/Source/CommandlineBatcher/Program.cs  
**Configure**  
https://github.com/sundews/CommandLineBatcher/blob/main/Source/CommandlineBatcher/BatchArguments.cs

### Nesting
**Usage**  
https://github.com/sundews/Aupli/blob/main/Source/Aupli/Program.cs  
**Configure**  
https://github.com/sundews/Aupli/blob/main/Source/Aupli/CommandLine/Options.cs
https://github.com/sundews/Aupli/blob/main/Source/Aupli/CommandLine/FileLogOptions.cs

### Nested verbs
**Usage**  
https://github.com/sundews/Sundew.Packaging/blob/main/Source/Sundew.Packaging.Tool/Program.cs  
**Configure**  
https://github.com/sundews/Sundew.Packaging/blob/main/Source/Sundew.Packaging.Tool/PruneLocalSourceVerb.cs
https://github.com/sundews/Sundew.Packaging/blob/main/Source/Sundew.Packaging.Tool/PruneLocalSource/AllVerb.cs