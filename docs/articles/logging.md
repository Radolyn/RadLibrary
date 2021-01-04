# Logging

In order to get a logger, you have to use `LogManager` class.

Example of getting logger with name of current class:

```csharp
var logger = LogManager.GetClassLogger();
```

You can check all available methods at <xref href="RadLibrary.Logging.LoggerBase" altProperty="LoggerBase"/>

If you want to configure logger, use <xref href="RadLibrary.Logging.LoggerSettings" altProperty="LoggerSettings"/>

All available methods for creating logger are defined in <xref href="RadLibrary.Logging.LogManager" altProperty="LogManager"/>
