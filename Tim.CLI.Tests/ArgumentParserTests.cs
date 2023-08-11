using System.Collections.Immutable;
using Tim.CLI.Business;

namespace Tim.CLI.Tests;

public class ArgumentParserTests
{
    [Test]
    public void Parse_HandlesRequiredArgs()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.Multiple(() =>
        {
            Assert.That(arguments.Start, Is.EqualTo(TimeOnly.FromTimeSpan(TimeSpan.FromHours(8))));
            Assert.That(arguments.End, Is.EqualTo(TimeOnly.FromTimeSpan(TimeSpan.FromHours(17))));
            Assert.That(arguments.Lunch, Is.EqualTo(TimeSpan.FromMinutes(30)));
            Assert.That(arguments.MainProjectLabel, Is.EqualTo("Label"));
        });
    }

    [Test]
    public void Parse_ParsesWorkDayHours()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-b", "7" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.WorkDayHours, Is.EqualTo(7.0));
    }

    [Test]
    public void Parse_DefaultsToEightWorkDayHours()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.WorkDayHours, Is.EqualTo(8.0));
    }
}
