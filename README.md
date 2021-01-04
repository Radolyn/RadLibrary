# RadLibrary

[![Codacy Badge](https://img.shields.io/codacy/grade/dbd6443c52e6446086f30d4ea9a95223?style=flat-square)](https://app.codacy.com/gh/Radolyn/RadLibrary/dashboard)
[![CodeFactor](https://img.shields.io/codefactor/grade/github/radolyn/radlibrary?style=flat-square)](https://www.codefactor.io/repository/github/radolyn/radlibrary)
[![NuGet](https://img.shields.io/nuget/v/radlibrary?style=flat-square)](https://www.nuget.org/packages/RadLibrary/)

All-In-One library

## Features

- Customizable logger
  - Prints dictionaries and lists like in ```Python```
- Colorful console support
  - Custom colors (**HEX string->Color class convert support** through ```Colorizer```)
  - ESC colors access through ```Font, Background & Foreground``` classes
  - ```Console``` wrapper for ease of use (```RadConsole```)
  - Colorful **input**
- Configuration manager
  - Comments support
  - Scheme support
- Formatters
  - Any types of Enumerables
  - Exception
  - String
  - __Custom__
- Utilities
  - Dynamic console allocation for GUI apps
  - Only one app instance
  - Easy to use random (int, bool) + random item from collection

## Getting started

Install RadLibrary in your project through [NuGet package](https://www.nuget.org/packages/RadLibrary/) ```RadLibrary```

If you're on .NET 5, you don't need to initialize anything, just write the code! Otherwise, call `Utilities.Initialize()` somewhere in `Main()`

## Documentation

**Check documentation at [our site](https://radolyn.com/docs/RadLibrary/)**

## Samples

![Sample image](.github/colorful_console.png)

![Sample image](.github/multilogger_console.png)

![Sample image](.github/multilogger_notepad.png)

![Sample image](.github/cfg_notepad.png)
