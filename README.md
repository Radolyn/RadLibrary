# RadLibrary

[![NuGet](https://img.shields.io/nuget/v/RadLibrary.svg)](https://www.nuget.org/packages/RadLibrary/)

All-In-One library

## Features

- Customizable logger
  - Custom colors
  - Prints dictionaries and lists like in Python
  - Integrated with configuration manager
  - Thread-safe
- Configuration manager
  - Comments support
  - Casts support

## Getting started

### Logging sample

```csharp
var logger = LoggerUtils.GetLogger("RadTest");

logger.Verbose("Verbose");
logger.Info("Info");
logger.Warn("Warn");
logger.Error("Error");
logger.Exception(new Exception());
logger.Verbose(new List<string> { "Yes", "No", "Maybe" });
logger.Info("{\"song\":\"Song name\",\"artist\":\"Radolyn\",\"start\":0,\"end\":9999999999999,\"paused\":false}");
```

![Sample image](https://radolyn.com/shared/radlibrary_1.png)

### Logging sample with custom settings

```csharp
var logger = LoggerUtils.GetLogger("RadTest");

logger.Settings.FormatJsonLike = false;
logger.Settings.InformationColor = Color.Teal;
logger.Settings.ExceptionColor = Color.DeepSkyBlue;

logger.Verbose("Verbose");
logger.Info("Info");
logger.Warn("Warn");
logger.Error("Error");
logger.Exception(new Exception());
logger.Verbose(new List<string> { "Yes", "No", "Maybe" });

logger.Info(
"{\"song\":\"Song name\",\"artist\":\"Radolyn\",\"start\":0,\"end\":9999999999999,\"paused\":false}");
```

### Configuration sample

```csharp
var logger = LoggerUtils.GetLogger("RadTest");
var config = AppConfiguration.Initialize<FileManager>("tester");

config["ip"] = "127.0.0.1";
config.SetBool("autorun", true);
config["url"] = "https://radolyn.com";

config.SetComment("ip", "Server IP");
config.SetComment("autorun", "Start program on Windows start");
config.SetComment("url", "Our main site");

config.Save();

logger.Info(config);
```

![Sample image](https://radolyn.com/shared/radlibrary_3.png)

![Sample image](https://radolyn.com/shared/radlibrary_4.png)
