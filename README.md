# Work day calculator

Console app that can be built as a nuget package and installed as a global tool.

There's no other intended users than myself. The repo is public because I need to access it from computers where I do not want to provide my github credentials. The business logic is tightly coupled to input requirements of a specific time reporting system.

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
dotnet pack
dotnet tool install --global --add-source ./nupkg Tim.CLI
```

## Usage

Once installed as a global tool, the program is executed by the name `tim` (as specified by the relevant csproj setting).

```
$ tim 0700 1800 0.5 MainProject -f -1 --OtherProject 2.5 ++Training 2 ++MainProject 0.5 -b 7
[12 T | MainProject 7.5 | OtherProject 2.5 | Training 2 | Flex 5 ]
```

Above means:
- You've worked from 07:00 to 18:00, with 0.5 hour lunch, mainly billing towards MainProject.
- During the day, you took one hour from your flex-account (-f -1) instead of working towards the MainProject.
- During the day, you worked 2.5 hours which should be billed towards OtherProject instead of MainProject (--OtherProject 2.5)
- After 18:00, you did 2 hours of Training, which should not be deducted from the MainProject hours, but which should add towards your flex-account
- After 18:00, you worked half an hours towards the MainProject (++MainProject 0.5), which should also add towards your flex-account
- The basis for flex calculations is the workday length, which defaults to 8 hours but is in this case manually overridden to 7 hours (-b 7)
- The output states that you worked 12 hours total, of which 7.5 hours should be billed towards MainProject, 2.5 hours towards OtherProject, 2 hours should be reported as Training. This workday yielded 5 hours towards the flex account.

## Help

```
tim --help
```

See unit tests for more examples.