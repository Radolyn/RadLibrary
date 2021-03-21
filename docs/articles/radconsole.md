# RadConsole

Wrapper for default Console class. Includes color and formatting support, custom input, etc.

```csharp
using Console = RadLibrary.RadConsole.RadConsole;

Console.WriteLine("[fffaaa]Your name is {0}!", s);
```

## Input

```csharp
using Console = RadLibrary.RadConsole.RadConsole;

var str = Console.Read.Line();
var str2 = Console.Read.Line("Enter your name: ");

var b = Console.Read.Boolean("Are you sure?");
var i = Console.Read.Integer("How many?");

```

You can check all available methods at <xref href="RadLibrary.RadConsole.RadConsole" altProperty="RadConsole"/>
