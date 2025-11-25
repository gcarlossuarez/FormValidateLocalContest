
# FormValidateLocalContest

## Purpose

This project is a Windows WinForms application designed to locally validate programming contest solutions. Its main features are:

- Display the description of programming problems stored in `.docx` files (including tables) in a multiline TextBox.
- Allow the user to load and compile C# source code for each problem.
- Automatically run tests using input/output files, comparing the program's output with the expected output.
- Fully support special characters and accents (UTF-8) throughout the workflow.
- Work without paid dependencies, using only free libraries and the .NET runtime.

## Dependencies

- .NET 10.0 (Windows)
- [DocumentFormat.OpenXml](https://www.nuget.org/packages/DocumentFormat.OpenXml) (NuGet package)
- Problems.zip (embedded resource with the problem datasets)

## How to create a self-contained release

You can generate a self-contained executable (does not require .NET to be installed on the target PC) using the following command in PowerShell or CMD:

```
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

- `-c Release`: Build in Release mode.
- `-r win-x64`: Publish for Windows 64-bit. You can use `win-x86` for 32-bit if needed.
- `--self-contained true`: Includes the .NET runtime in the executable.
- `/p:PublishSingleFile=true`: Generates a single executable file.

The final executable will be located in:

```
FormValidateLocalContest\bin\Release\net10.0-windows\win-x64\publish\
```

You only need to distribute that `.exe` (and Problems.zip if you want it external, but here it is already embedded).

## Additional notes

- The validator is prepared to correctly handle input and output files in UTF-8 without BOM.
- If the user's program prints in another encoding, check the code comments to adjust the decoding.
- The Problems.zip resource is embedded in the executable, so you do not need to distribute it separately.

---

If you have questions or need to customize the publishing process, see the official .NET documentation: https://learn.microsoft.com/dotnet/core/deploying/
