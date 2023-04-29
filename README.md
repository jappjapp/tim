# Work day calculator

Console app that can be built as a nuget package and installed as a global tool.

## Installation

```
cd Tim.CLI
dotnet pack
dotnet tool uninstall --global Tim.CLI
dotnet tool install --global --add-source ./nupkg Tim.CLI
```