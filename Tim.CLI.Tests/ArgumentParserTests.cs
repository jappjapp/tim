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
    public void Parse_HandlesThreeDifferentFlex()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75", "-f", "-0.5", "-f", "2" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(2.25)));
    }

    #endregion FlexFlags

    #region ProjectFlagsDuringWorkday

    [Test]
    public void Parse_HandlesOneProjectFlagDuringWorkday()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "--MockProject1", "2.75" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.ProjectHoursDuringWorkday, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(arguments.ProjectHoursDuringWorkday.First().Key, Is.EqualTo("MockProject1"));
            Assert.That(arguments.ProjectHoursDuringWorkday.First().Value, Is.EqualTo(2.75));
        });
    }

    [Test]
    public void Parse_HandlesThreeProjectFlagsDuringWorkday()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label",
            "--MockProject1", "1.75",
            "--MockProject2", "2.75",
            "--MockProject3", "3.75"
        }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.ProjectHoursDuringWorkday, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(arguments.ProjectHoursDuringWorkday.First().Key, Is.EqualTo("MockProject1"));
            Assert.That(arguments.ProjectHoursDuringWorkday.First().Value, Is.EqualTo(1.75));

            Assert.That(arguments.ProjectHoursDuringWorkday.Skip(1).First().Key, Is.EqualTo("MockProject2"));
            Assert.That(arguments.ProjectHoursDuringWorkday.Skip(1).First().Value, Is.EqualTo(2.75));

            Assert.That(arguments.ProjectHoursDuringWorkday.Last().Key, Is.EqualTo("MockProject3"));
            Assert.That(arguments.ProjectHoursDuringWorkday.Last().Value, Is.EqualTo(3.75));
        });
    }

    [Test]
    public void Parse_HandlesDuplicateProjectFlagsDuringWorkday()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label",
            "--MockProject1", "1.75",
            "--MockProject2", "2.75",
            "--MockProject1", "-0.75"
        }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.ProjectHoursDuringWorkday, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(arguments.ProjectHoursDuringWorkday.First().Key, Is.EqualTo("MockProject1"));
            Assert.That(arguments.ProjectHoursDuringWorkday.First().Value, Is.EqualTo(1));

            Assert.That(arguments.ProjectHoursDuringWorkday.Last().Key, Is.EqualTo("MockProject2"));
            Assert.That(arguments.ProjectHoursDuringWorkday.Last().Value, Is.EqualTo(2.75));
        });
    }

    #endregion ProjectFlagsDuringWorkday

    #region ProjectFlagsOutsideWorkday

    [Test]
    public void Parse_HandlesOneProjectFlagOutsideWorkday()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "++MockProject1", "2.75" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.ProjectHoursOutsideWorkday, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(arguments.ProjectHoursOutsideWorkday.First().Key, Is.EqualTo("MockProject1"));
            Assert.That(arguments.ProjectHoursOutsideWorkday.First().Value, Is.EqualTo(2.75));
        });
    }

    [Test]
    public void Parse_HandlesThreeProjectFlagsOutsideWorkday()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label",
            "++MockProject1", "1.75",
            "++MockProject2", "2.75",
            "++MockProject3", "3.75"
        }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.ProjectHoursOutsideWorkday, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(arguments.ProjectHoursOutsideWorkday.First().Key, Is.EqualTo("MockProject1"));
            Assert.That(arguments.ProjectHoursOutsideWorkday.First().Value, Is.EqualTo(1.75));

            Assert.That(arguments.ProjectHoursOutsideWorkday.Skip(1).First().Key, Is.EqualTo("MockProject2"));
            Assert.That(arguments.ProjectHoursOutsideWorkday.Skip(1).First().Value, Is.EqualTo(2.75));

            Assert.That(arguments.ProjectHoursOutsideWorkday.Last().Key, Is.EqualTo("MockProject3"));
            Assert.That(arguments.ProjectHoursOutsideWorkday.Last().Value, Is.EqualTo(3.75));
        });
    }

    [Test]
    public void Parse_HandlesDuplicateProjectFlagsOutsideWorkday()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label",
            "++MockProject1", "1.75",
            "++MockProject2", "2.75",
            "++MockProject1", "-0.75"
        }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.ProjectHoursOutsideWorkday, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(arguments.ProjectHoursOutsideWorkday.First().Key, Is.EqualTo("MockProject1"));
            Assert.That(arguments.ProjectHoursOutsideWorkday.First().Value, Is.EqualTo(1));

            Assert.That(arguments.ProjectHoursOutsideWorkday.Last().Key, Is.EqualTo("MockProject2"));
            Assert.That(arguments.ProjectHoursOutsideWorkday.Last().Value, Is.EqualTo(2.75));
        });
    }

    #endregion ProjectFlagsProjectFlagsOutsideWorkday


    #region CombinedCases

    [Test]
    public void Parse_HandlesFlexAndCustomWorkDayLength()
    {
        var args = new string[] { "0800", "1700", "0.5", "Label", "-f", "0.75", "-b", "6", "-f", "-1.5" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(-0.75)));
        Assert.That(arguments.WorkDayHours, Is.EqualTo(6.0));
    }

    [Test]
    public void Parse_HandlesReadmeExample()
    {
        var args = new string[] { "0700", "1800", "0.5", "MainProject", "-f", "-1", "--OtherProject", "2.5", "++Training", "2", "++MainProject", "0.5", "-b", "7" }.ToImmutableArray();

        var arguments = ArgumentHandler.Parse(args);

        Assert.Multiple(() =>
        {
            Assert.That(arguments.Start, Is.EqualTo(TimeOnly.FromTimeSpan(TimeSpan.FromHours(7))));
            Assert.That(arguments.End, Is.EqualTo(TimeOnly.FromTimeSpan(TimeSpan.FromHours(18))));
            Assert.That(arguments.Lunch, Is.EqualTo(TimeSpan.FromMinutes(30)));
            Assert.That(arguments.MainProjectLabel, Is.EqualTo("MainProject"));
            Assert.That(arguments.FlexHours, Is.EqualTo(TimeSpan.FromHours(-1.0)));
            Assert.That(arguments.WorkDayHours, Is.EqualTo(7.0));
            Assert.That(arguments.ProjectHoursDuringWorkday, Has.Count.EqualTo(1));
            Assert.That(arguments.ProjectHoursDuringWorkday.First(x => x.Key == "OtherProject").Value, Is.EqualTo(2.5));
            Assert.That(arguments.ProjectHoursOutsideWorkday.First(x => x.Key == "Training").Value, Is.EqualTo(2));
            Assert.That(arguments.ProjectHoursOutsideWorkday.First(x => x.Key == "MainProject").Value, Is.EqualTo(0.5));
        });
    }


    #endregion CombinedCases
}
