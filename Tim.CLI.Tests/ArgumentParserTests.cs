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

    #region FlexFlags

    [Test]
    public void Parse_HandlesOnePositiveFlex()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(0.75)));
    }

    [Test]
    public void Parse_HandlesTwoPositiveFlex()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75", "-f", "0.5" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(1.25)));
    }

    [Test]
    public void Parse_HandlesTwoDifferentFlex()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75", "-f", "-0.5" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(0.25)));
    }

    [Test]
    public void Parse_HandlesThreeoDifferentFlex()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75", "-f", "-0.5", "-f", "2" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(2.25)));
    }

    #endregion FlexFlags

    #region CombinedCases

    [Test]
    public void Parse_HandlesTwoFlexFlagsAndOneWorkDayFlag()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75", "-b", "6", "-f", "-1.5" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(-0.75)));
        Assert.That(arguments.WorkDayHours, Is.EqualTo(6.0));
    }

    #endregion CombinedCases
}
