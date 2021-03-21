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
    - String
    - Boolean
    - Integer
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

Check Articles for more information!

## Colorful console

### Write line to the console

*Pro tip: write `using Console = RadLibrary.RadConsole.RadConsole;` at usings and use `Console` class as always~*

```csharp
RadConsole.WriteLine("[#fffff]{CoolPrefix} [aaaff]Some colorful text");

RadConsole.WriteLine("[00ffcc]{CoolPrefix} [#ffff66]Some colorful text");
```

![Sample image](images/colorful_console.png)

## Logging

### Current method logger

```csharp
var logger = LogManager.GetMethodLogger();

logger.Trace("Trace");
logger.Debug("Debug");
logger.Info("Info");
logger.Warn("Warn");
logger.Error("Error");

try
{
  int.Parse("sadasd");
}
catch (Exception e)
{
  logger.Fatal(e);
}

logger.Info(new List<string> { "Yes", "No", "Maybe" });
logger.Info("{\"song\":\"Song name\",\"artist\":\"Radolyn\",\"start\":0,\"end\":9999999999999,\"paused\":false}");

```

![Sample image](images/logger_console.png)

### Multilogger sample

```csharp
var consoleLogger = LogManager.GetMethodLogger();
var fileLogger = LogManager.GetMethodLogger<FileLogger>();

var logger = LogManager.GetClassLogger<MultiLogger>(consoleLogger, fileLogger);

logger.Trace("Trace");
logger.Debug("Debug");
logger.Info("Info");
logger.Warn("Warn");
logger.Error("Error");

try
{
  int.Parse("sadasd");
}
catch (Exception e)
{
  logger.Fatal(e);
}

logger.Info(new List<string> { "Yes", "No", "Maybe" });
logger.Info("{\"song\":\"Song name\",\"artist\":\"Radolyn\",\"start\":0,\"end\":9999999999999,\"paused\":false}");

```

![Sample image](images/multilogger_console.png)
![Sample image](images/multilogger_notepad.png)

## Configuration Manager

### Create configuration using FileManager

```csharp
var logger = LogManager.GetMethodLogger();
var config = new IniManager("test.conf");

config["ip"] = "127.0.0.1";
config["autorun"] = true;
config["key"].SetValue('a');
config["url"] = "https://radolyn.com";

config.Save();
```
