using Tim.CLI.Business;
using Tim.CLI.Models;

namespace Tim.CLI.Tests;

public class WorkDayCalculatorTests
{
    [SetUp]
    public void Setup()
    {
    }

    #region SimpleWorkDay

    // Simple work day: Start, End, Lunch, Label

    [Test]
    public void Calculate_HandlesSimpleWorkDay1()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:00", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesSimpleWorkDay2()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:30", 1.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesSimpleWorkDay3()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "16:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion SimpleWorkDay

    #region PositiveFlex

    [Test]
    public void Calculate_HandlesLongerWorkday1()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:15", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.25));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesLongerWorkday2()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:00", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.5));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.5));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesLongerWorkday3()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:15", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(8.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(0.75));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesLongerWorkday4()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "17:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(9));
            Assert.That(workDayResult.Flex, Is.EqualTo(1));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion PositiveFlex

    #region NegativeFlex

    [Test]
    public void Calculate_HandlesShorterWorkday1()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "16:45", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.75));
            Assert.That(workDayResult.Flex, Is.EqualTo(-0.25));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }


    [Test]
    public void Calculate_HandlesShorterWorkday2()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "16:00", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.5));
            Assert.That(workDayResult.Flex, Is.EqualTo(-0.5));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesShorterWorkday3()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "16:15", 1, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.25));
            Assert.That(workDayResult.Flex, Is.EqualTo(-0.75));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesShorterWorkday4()
    {
        var args = CreateArgsWithDefaultWorkDay("08:00", "15:30", 0.5, "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.TotalHours, Is.EqualTo(7.0));
            Assert.That(workDayResult.Flex, Is.EqualTo(-1.0));
            Assert.That(workDayResult.MainLabel, Is.EqualTo("M"));
        });
    }

    #endregion NegativeFlex

    // Then add base hours

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

    //# test positive flex and projects
    //puts[isEqual { 0F | 6.5M | 1Training | 0.5Admin}
    //[wdaycalc 0800 1700 1 M 1Training 0.5Admin]]
    //puts[isEqual { 0.25F + | 6.75M | 1.0Training | 0.5Admin}
    //[wdaycalc 0800 1715 1 M 1.0Training 0.5Admin]]
    //puts[isEqual { 0.5F + | 7.0M | 1Training | 0.5Admin}
    //[wdaycalc 0800 1700 0.5 M 1Training 0.5Admin]]
    //puts[isEqual { 0.75F + | 7.25M | 1Training | 0.5Admin}
    //[wdaycalc 0800 1715 0.5 M 1Training 0.5Admin]]

    //# test negative flex and projects
    //puts[isEqual { 0.25F - | 6.75M | 0.75Training | 0.25Admin}
    //[wdaycalc 0800 1645 1 M 0.75Training 0.25Admin]]
    //puts[isEqual { 0.5F - | 6.5M | 0.75Training | 0.25Admin}
    //[wdaycalc 0800 1600 0.5 M 0.75Training 0.25Admin]]
    //puts[isEqual { 0.25F - | 6.75M | 0.75Training | 0.25Admin}
    //[wdaycalc 0800 1615 0.5 M 0.75Training 0.25Admin]]

    //# test extra negative flex in project hours
    //puts[isEqual { 0.75F - | 6.25M | 0.75Training | 0.25Admin}
    //[wdaycalc 0800 1715 1 M 1F- 0.75Training 0.25Admin]]
    //puts[isEqual { 1.75F - | 4.75M | 1.0Training | 0.5Admin}
    //[wdaycalc 0800 1615 1 M 1.0Training 0.5Admin 1F-]]


    private static Arguments CreateArgsWithDefaultWorkDay(string startTime, string endTime, double lunchHours, string mainProjectLabel)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: 8.0); // Default
    }

    private static Arguments CreateArgsWithCustomWorkDayLength(string startTime, string endTime, double lunchHours, string mainProjectLabel, double workDayLength)
    {
        return new Arguments(
            Start: TimeOnly.Parse(startTime),
            End: TimeOnly.Parse(endTime),
            Lunch: TimeSpan.FromHours(lunchHours),
            MainProjectLabel: mainProjectLabel,
            WorkDayHours: workDayLength);
    }

}