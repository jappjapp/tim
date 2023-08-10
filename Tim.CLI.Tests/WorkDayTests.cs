using Tim.CLI.Business;
using Tim.CLI.Models;

namespace Tim.CLI.Tests;

public class WorkDayTests
{
    [SetUp]
    public void Setup()
    {
    }

    // Simple work day: Start, End, Lunch, Label

    [Test]
    public void Calculate_HandlesSimpleWorkDay1()
    {
        var args = new Arguments(Start: TimeOnly.Parse("08:00"), End: TimeOnly.Parse("17:00"), Lunch: TimeSpan.FromHours(1), MainProjectLabel: "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.totalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.mainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesSimpleWorkDay2()
    {
        var args = new Arguments(Start: TimeOnly.Parse("08:00"), End: TimeOnly.Parse("17:30"), Lunch: TimeSpan.FromHours(1.5), MainProjectLabel: "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.totalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.mainLabel, Is.EqualTo("M"));
        });
    }

    [Test]
    public void Calculate_HandlesSimpleWorkDay3()
    {
        var args = new Arguments(Start: TimeOnly.Parse("08:00"), End: TimeOnly.Parse("16:30"), Lunch: TimeSpan.FromHours(0.5), MainProjectLabel: "M");

        var workDayResult = WorkDayCalculator.Calculate(args);

        Assert.Multiple(() =>
        {
            Assert.That(workDayResult.totalHours, Is.EqualTo(8.0));
            Assert.That(workDayResult.mainLabel, Is.EqualTo("M"));
        });
    }

    // Positive flex

    //puts[isEqual { 0.25F + | 8.25M} [wdaycalc 0800 1715 1 M]]
    //puts[isEqual { 0.5F + | 8.5M} [wdaycalc 0800 1700 0.5 M]]
    //puts[isEqual { 0.75F + | 8.75M} [wdaycalc 0800 1715 0.5 M]]
    //puts[isEqual { 1.0F + | 9.0M} [wdaycalc 0800 1730 0.5 M]]

    // Negative flex

    //puts[isEqual { 0.25F - | 7.75M}
    //[wdaycalc 0800 1645 1 M]]
    //puts[isEqual { 0.5F - | 7.5M}
    //[wdaycalc 0800 1600 0.5 M]]
    //puts[isEqual { 0.75F - | 7.25M}
    //[wdaycalc 0800 1615 1 M]]
    //puts[isEqual { 1.0F - | 7.0M}
    //[wdaycalc 0800 1530 0.5 M]]

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



}