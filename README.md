# Sundew.CommandLine

[![Build Status](https://drosera.visualstudio.com/Sundew.CommandLine/_apis/build/status/hugener.Sundew.CommandLine?branchName=master)](https://drosera.visualstudio.com/Sundew.CommandLine/_build/latest?definitionId=1&branchName=master)
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
https://github.com/hugener/Sundew.Packaging.Tool/blob/main/Source/Sundew.Packaging.Tool/Program.cs  
**Configure**  
https://github.com/hugener/Sundew.Packaging.Tool/blob/main/Source/Sundew.Packaging.Tool/UpdateVerb.cs

### Required value
**Configure**  
https://github.com/hugener/Sundew.Packaging.Tool/blob/main/Source/Sundew.Packaging.Tool/AwaitPublishVerb.cs

### Required list, require any of and enum
**Usage**  
https://github.com/hugener/CommandLineBatcher/blob/main/Source/CommandlineBatcher/Program.cs  
**Configure**  
https://github.com/hugener/CommandLineBatcher/blob/main/Source/CommandlineBatcher/BatchArguments.cs

### Nesting
**Usage**  
https://github.com/hugener/Aupli/blob/master/Source/Aupli/Program.cs  
**Configure**  
https://github.com/hugener/Aupli/blob/master/Source/Aupli/CommandLine/Options.cs
https://github.com/hugener/Aupli/blob/master/Source/Aupli/CommandLine/FileLogOptions.cs

### Nested verbs
**Usage**  
https://github.com/hugener/Sundew.Packaging.Tool/blob/main/Source/Sundew.Packaging.Tool/Program.cs  
**Configure**  
https://github.com/hugener/Sundew.Packaging.Tool/blob/main/Source/Sundew.Packaging.Tool/PruneLocalSourceVerb.cs
https://github.com/hugener/Sundew.Packaging.Tool/blob/main/Source/Sundew.Packaging.Tool/PruneLocalSource/AllVerb.cs