# Utilities

It ain't much, but it's honest work.

## Random

```csharp
var i = Utilities.RandomInt(0, 100);
var b = Utilities.RandomBool();

var list = new List<string>
{
    "1",
    "2"
};
var randomItem = list.RandomItem(); // <- via extension method
```

## Only one instance

```csharp
Utilities.OnlyOneInstance("MyAwesomeApp");
```

## Allocate console

Useful for Windows Forms or WPF apps

```csharp
Utilities.AllocateConsole();
```

You can check all available methods at <xref href="RadLibrary.RadExtensions" altProperty="RadExtensions"/> or <xref href="RadLibrary.Utilities" altProperty="Utilities"/>
