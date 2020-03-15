# RadLibrary
All-In-One library

# Logging sample

```csharp
var logger = LoggerUtils.GetLogger("Program");
logger.Verbose("Verbose");
logger.Info("Info");
logger.Warn("Warn");
logger.Error("Error");
logger.Verbose(new List<string> { "Yes", "No", "Maybe" });
```

# Logging sample with custom settings
```csharp
var logger = LoggerUtils.GetLogger("Program");
var settings = new LoggerSettings
{
    LogLevel = LogType.Information,
    InformationColor = ConsoleColor.DarkCyan
};
logger.Settings = settings;
logger.Info("New color test");
```
