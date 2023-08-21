using Tim.CLI.Business;
using Tim.CLI.Models;

namespace Tim.CLI.Tests;

public class WorkDayCalculatorTests
{
    [SetUp]
    public void Setup()
    {
    }

    #region LunchLengths

    [Test]
    public void Calculate_HandlesStandardDay()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:00", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(8.0));
        });
    }

    [Test]
    public void Calculate_HandlesHandlesLongLunch()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:30", 1.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(8.0));
        });
    }

    [Test]
    public void Calculate_HandlesShortLunch()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "16:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(8.0));
        });
    }

    #endregion LunchLengths

    #region LongerWorkDays

    [Test]
    public void Calculate_LongerWorkdayYieldsCorrectHours1()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:15", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.25));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(8.25));
        });
    }

    [Test]
    public void Calculate_LongerWorkdayYieldsCorrectHours2()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:15", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.75));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(8.75));
        });
    }

    #endregion LongerWorkDays

    #region ShorterWorkDays

    [Test]
    public void Calculate_ShorterWorkdayYieldsCorrectHours1()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "16:45", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(-0.25));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(7.75));
        });
    }

    [Test]
    public void Calculate_ShorterWorkdayYieldsCorrectHours2()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "15:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.0));
            Assert.That(workDayResult.Flex, Is.EqualTo(-1.0));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(7.0));
        });
    }

    #endregion ShorterWorkDays

    #region CustomWorkDayLength

    [Test]
    public void Calculate_HonorsWorkDayLengthArgument()
    {
        var args = CreateArgsWithCustomWorkDayLength("08:00", "16:00", 1, "M", 7.0);

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.0));
        });
    }

    [Test]
    public void Calculate_HonorsWorkDayLengthArgumentWithPositiveFlex()
    {
        var args = CreateArgsWithCustomWorkDayLength("08:00", "16:15", 1, "M", 7.0);

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.25));
        });
    }

    [Test]
    public void Calculate_HonorsWorkDayLengthArgumentWithNegativeFlex()
    {
        var args = CreateArgsWithCustomWorkDayLength("08:00", "16:00", 1.25, "M", 7.0);

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(6.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(-0.25));
        });
    }

    #endregion CustomWorkDayLength

    #region ExplicitFlexHours

    [Test]
    public void Calculate_PositiveFlexHoursYieldsCorrectHours()
    {
        var args = CreateArgsWithCustomWorkDayLengthAndFlex("08:00", "17:15", 0.5, "M", 8.0, 0.5);

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(9.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(1.25));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(9.25));
        });
    }

    [Test]
    public void Calculate_NegativeFlexHoursYieldsCorrectHours()
    {
        var args = CreateArgsWithCustomWorkDayLengthAndFlex("08:00", "17:15", 0.5, "M", 8.0, -0.5);

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.25));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(1));
            Assert.That(workDayResult.SpecifiedHours.First().Key, Is.EqualTo("M"));
            Assert.That(workDayResult.SpecifiedHours.First().Value, Is.EqualTo(8.25));
        });
    }

    #endregion ExplicitFlexHours

    #region MultipleProjects

    [Test]
    public void Calculate_OneProjectYieldsCorrectHours()
    {
        var args = CreateArgsWithProjects("08:00", "17:00", 1, "M", new Dictionary<string, double>() {
            { "Admin", 1.0 }
        });

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.0));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(2));
            Assert.That(workDayResult.SpecifiedHours.First(x => x.Key.Equals("M")).Value, Is.EqualTo(7.0));
            Assert.That(workDayResult.SpecifiedHours.First(x => x.Key.Equals("Admin")).Value, Is.EqualTo(1.0));
        });
    }

    [Test]
    public void Calculate_ThreeProjectsYieldCorrectHours()
    {
        var args = CreateArgsWithProjects("08:00", "17:00", 1, "M", new Dictionary<string, double>() {
            { "Admin", 2.0 },
            { "UPL", 1.0 },
            { "Training", 3.0 },
        });

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.0));
            Assert.That(workDayResult.SpecifiedHours, Has.Count.EqualTo(4));
            Assert.That(workDayResult.SpecifiedHours.First(x => x.Key.Equals("M")).Value, Is.EqualTo(2.0));
            Assert.That(workDayResult.SpecifiedHours.First(x => x.Key.Equals("Admin")).Value, Is.EqualTo(2.0));
            Assert.That(workDayResult.SpecifiedHours.First(x => x.Key.Equals("UPL")).Value, Is.EqualTo(1.0));
            Assert.That(workDayResult.SpecifiedHours.First(x => x.Key.Equals("Training")).Value, Is.EqualTo(3.0));
        });
    }

    #endregion MultipleProjects

    // CombinedCases
    // Flex plus customwdaylength
    // Flex plus flera projekt, inkl kasta om argument
    // Flera project och customwdaylength

    // Todo: move to own file
    [Test]
    public void Validate_HandlesProjectsGreaterThanTotalHours()
    {

    }

    private static Arguments CreateArgsWithDefaultWorkDay(string startTime, string endTime, double lunchHours, string mainProjectLabel)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: Constants.DefaultWorkDayHours,
            FlexHours: TimeSpan.Zero,
            ProjectHoursDuringWorkday: new(),
            ProjectHoursOutsideWorkday: new());
    }

    private static Arguments CreateArgsWithCustomWorkDayLength(string startTime, string endTime, double lunchHours, string mainProjectLabel, double workDayLength)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: workDayLength,
            FlexHours: TimeSpan.Zero,
            ProjectHoursDuringWorkday: new(),
            ProjectHoursOutsideWorkday: new());
    }

    private static Arguments CreateArgsWithCustomWorkDayLengthAndFlex(string startTime, string endTime, double lunchHours, string mainProjectLabel, double workDayLength, double flexHours)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: workDayLength,
            FlexHours: TimeSpan.FromHours(flexHours),
            ProjectHoursDuringWorkday: new(),
            ProjectHoursOutsideWorkday: new());
    }

    private static Arguments CreateArgsWithProjects(string startTime, string endTime, double lunchHours, string mainProjectLabel, Dictionary<string, double> projectHours)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: Constants.DefaultWorkDayHours,
            FlexHours: TimeSpan.Zero,
            ProjectHoursDuringWorkday: projectHours,
            ProjectHoursOutsideWorkday: new());
    }
}