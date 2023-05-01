# Work day calculator

Console app that can be built as a nuget package and installed as a global tool.

Because reasons, `Program.cs` is written with the intention of being manually copied into a new console app, instead of cloning the repo.

There's no other intended users than myself. The repo is public because I need to access it from computers where I do not want to provide my github credentials. The business logic is heavily coupled to input requirements of a specific time reporting system.

## Installation

```
cd Tim.CLI
dotnet pack
dotnet tool install --global --add-source ./nupkg Tim.CLI
```

To install a new version of the tool. (Keeping it as simple as possible. Not using versioning for this app.)

```
dotnet tool uninstall --global Tim.CLI
cd Tim.CLI
dotnet tool install --global --add-source ./nupkg Tim.CLI
```

## Usage

Once installed as a global tool, the program is executed by the name `tim`, as specified by the relevant csproj setting.

```
$ tim 0700 1800 0.5 MainProjectName -n 1 --OtherProjectName 2.5 -n 0.5 +m 1 ++OtherProjectName 2
[12 T | 7.5 main | 4.5 other | 5 F+ ]
```
Above means:
- You've worked from 07:00 to 18:00, with 0.5 hour lunch
	- Put all time towards "MainProjectName", but deduct the following
		- Negative flex (-n), 1 + 0.5 = 1.5 hours.
		- Other projects (--ProjectName), here 2.5 hours for "OtherProjectName".
- Any additional time outside of 07:00 to 18:00 use + sign for arguments
	- Towards main project (+m). Here 1 hour
	- Towards other projects (++ProjectName), here 2 hours
- Positive flex is calculated when total is more than 8 hours. This baseline work day length can be set with argument -b and defaults to 8 hours.

In this example, output should be  
11 - 0.5L - 1n - 0.5n + 1m + 2OtherProjectName  
Total 12 hours  
baseline 7 gives 12 - 7 = 5 hours flexplus  
11 - 0.5L - 1n - 0.5n + 1m - 2.5Other = 7.5 main  
2.5 + 2 = 4.5 other  

See unit tests for more examples.