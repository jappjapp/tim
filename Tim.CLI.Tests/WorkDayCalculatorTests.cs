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
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "17:00", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesHandlesLongLunch()
    {
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "17:30", 1.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesShortLunch()
    {
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "16:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion LunchLengths

    #region LongerWorkDays

    [Test]
    public void Calculate_LongerWorkdayYieldsCorrectHours1()
    {
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "17:15", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.25));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_LongerWorkdayYieldsCorrectHours2()
    {
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "17:15", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.75));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion LongerWorkDays

    #region ShorterWorkDays

    [Test]
    public void Calculate_ShorterWorkdayYieldsCorrectHours1()
    {
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "16:45", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(-0.25));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_ShorterWorkdayYieldsCorrectHours2()
    {
        var args = CreateArgsWithDefaultWorkDayAndNoFlex("08:00", "15:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.0));
            Assert.That(workDayResult.Flex, Is.EqualTo(-1.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion ShorterWorkDays

    #region CustomWorkDayLength

    [Test]
    public void Calculate_HonorsWorkDayLengthArgument()
    {
        var args = CreateArgsWithCustomWorkDayLengthAndNoFlex("08:00", "16:00", 1, "M", 7.0);

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.0));
        });
    }

    [Test]
    public void Calculate_HonorsWorkDayLengthArgumentWithPositiveFlex()
    {
        var args = CreateArgsWithCustomWorkDayLengthAndNoFlex("08:00", "16:15", 1, "M", 7.0);

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
        var args = CreateArgsWithCustomWorkDayLengthAndNoFlex("08:00", "16:00", 1.25, "M", 7.0);

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
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
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
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion ExplicitFlexHours

    // MoreThanOneProject

    // CombinedCases
    // Flex plus customwdaylength
    // Flex plus flera projekt, inkl kasta om argument
    // Flera project och customwdaylength
    // 



    private static Arguments CreateArgsWithDefaultWorkDayAndNoFlex(string startTime, string endTime, double lunchHours, string mainProjectLabel)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: 8.0, // Default as set by argument parser
            FlexHours: TimeSpan.Zero);
    }

    private static Arguments CreateArgsWithCustomWorkDayLengthAndNoFlex(string startTime, string endTime, double lunchHours, string mainProjectLabel, double workDayLength)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: workDayLength,
            FlexHours: TimeSpan.Zero);
    }

    private static Arguments CreateArgsWithCustomWorkDayLengthAndFlex(string startTime, string endTime, double lunchHours, string mainProjectLabel, double workDayLength, double flexHours)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: workDayLength,
            FlexHours: TimeSpan.FromHours(flexHours));
    }


}