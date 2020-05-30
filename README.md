<div align=center>
 <img alt="EzColors" src="console.png" width="60%">
 <br>
 <img alt="Nuget" src="https://img.shields.io/nuget/v/EzConsole">
 <img alt="Nuget" src="https://img.shields.io/nuget/dt/EzConsole">
</div>


# EzConsole

EzConsole is a collection of modules included in the EzConsole NuGet Package, this is all bundled together to form this complete set of better functionalities added and wrapped around the `System.Console` class.

Here is an overview of all the modules added in the EzConsole Collection Package:

---

<div align=center>
 <img alt="EzColors" src="colors.png" width="30%">
</div>

## EzColors
A .NET Standard library built for simple and accessible command line argument parsing, manual instructions and validation.

### Usage

#### Examples

##### 1. Console Static Replacement

A drop-in static replacement for `System.Console`'s write methods.

***Example:***

You can use via 

````csharp
EzConsole.Write(string text, ConsoleColor foreground, ConsoleColor background)
````

or

***Example:***

````csharp
using Console = EzConsole.EzConsole;
````

This will not require any change in your program. Everything you need to do now is changing the `Write` calls that you want colorized!
