
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

## Publishing

The application supports multiple .NET versions (.NET 8, 9, and 10). You can generate publications for any of them.

### General Publish Command

```powershell
dotnet publish -c Release --framework <FRAMEWORK> -o <OUTPUT_PATH>
```

### Options

| Option | Description |
|--------|------------|
| `-c Release` | Builds in Release mode (optimized, no debug) |
| `--framework <FRAMEWORK>` | Specifies which .NET version to publish for |
| `-o <PATH>` | Specifies where to save the publication |

### Examples

**For .NET 8:**
```powershell
cd "D:\Universidad Católica Boliviana\migration-to-the-cloud\analisis_nube\fuentes\FormValidateLocalContest\FormValidateLocalContest"
dotnet publish -c Release --framework net8.0-windows -o "bin\Release\publish\net8.0"
```

**For .NET 9:**
```powershell
dotnet publish -c Release --framework net9.0-windows -o "bin\Release\publish\net9.0"
```

**For .NET 10:**
```powershell
dotnet publish -c Release --framework net10.0-windows -o "bin\Release\publish\net10.0"
```

**Generate all versions at once:**
```powershell
dotnet publish -c Release
```

This last option will automatically generate all three versions in:
- `bin\Release\net8.0-windows\`
- `bin\Release\net9.0-windows\`
- `bin\Release\net10.0-windows\`

### Generated Files

Each publication includes:
- `FormValidateLocalContest.exe` - Application executable
- `FormValidateLocalContest.dll` - Main library
- `FormValidateLocalContest.pdb` - Debug information (optional)
- `FormValidateLocalContest.deps.json` - Dependencies
- `FormValidateLocalContest.runtimeconfig.json` - Runtime configuration
- External dependencies (e.g., `DocumentFormat.OpenXml.dll`)

## How to create a self-contained release

You can generate a self-contained executable (does not require .NET to be installed on the target PC) using the following command in PowerShell or CMD:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true --framework net10.0-windows
```

- `-c Release`: Build in Release mode.
- `-r win-x64`: Publish for Windows 64-bit. You can use `win-x86` for 32-bit if needed.
- `--self-contained true`: Includes the .NET runtime in the executable.
- `/p:PublishSingleFile=true`: Generates a single executable file.
- `--framework net10.0-windows`: Specifies the .NET version (use `net8.0-windows` or `net9.0-windows` for other versions).

The final executable will be located in:

```Powershell
FormValidateLocalContest\bin\Release\net10.0-windows\win-x64\publish\
```

You only need to distribute that `.exe` (and Problems.zip if you want it external, but here it is already embedded).

## Additional notes

- The validator is prepared to correctly handle input and output files in UTF-8 without BOM.
- If the user's program prints in another encoding, check the code comments to adjust the decoding.
- The Problems.zip resource is embedded in the executable, so you do not need to distribute it separately.

---

If you have questions or need to customize the publishing process, see the official .NET documentation: <https://learn.microsoft.com/dotnet/core/deploying/>
