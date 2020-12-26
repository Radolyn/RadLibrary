# Logging

In order to get a logger, you have to use `LogManager` class.

Example of getting logger with name of current class:

```csharp
var logger = LogManager.GetClassLogger();
```

You can check all available methods at @"RadLibrary.Logging.LoggerBase"

If you want to configure logger, use @"RadLibrary.Logging.LoggerSettings"

All available methods for creating logger are defined in @"RadLibrary.Logging.LogManager"
