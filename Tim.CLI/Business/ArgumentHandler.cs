using System.Collections.Immutable;
using Tim.CLI.Models;

namespace Tim.CLI.Business;
internal static class ArgumentHandler
{
    internal static Arguments Parse(ImmutableArray<string> args)
    {
        // Start with:
        // 0800 1700 0.5 Label

        var start = TimeOnly.Parse(args[0].Insert(2, ":"));
        var end = TimeOnly.Parse(args[1].Insert(2, ":"));
        var lunch = TimeSpan.FromHours(double.Parse(args[2]));
        var label = args[3];

        return new(start, end, lunch, label);

        // Later:
        // $ tim 0700 1800 0.5 MainProjectName -n 1 --OtherProjectName 2.5 -n 0.5 +m 1 ++OtherProjectName 2
        // [12 T | 7.5 main | 4.5 other | 5 F + ]
        // start, end, lunch, mainprojectlabel
        // key value list of enum and hour
    }

    internal static List<string> Validate(ImmutableArray<string> args)
    {
        throw new NotImplementedException();
    }
}
