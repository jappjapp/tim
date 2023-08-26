using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.CLI.Business;
using Tim.CLI.Models;

namespace Tim.CLI.Tests;
public class ResultFormatterTests
{
    [Test]
    public void FormatResult_FormatsNormalWorkDay()
    {
        var result = new WorkDayCalcResult(TotalHours: 8.0, MainProjectLabel: "M", Flex: 0.0, SpecifiedHours: new() { { "M", 8.0 } });

        var formattedResult = ResultFormatter.FormatResultToLineString(result);

        Assert.That(formattedResult, Is.EqualTo("[ M 8 | Total 8 ]"));
    }

    [Test]
    public void FormatResult_FormatsPositiveFlex()
    {
        var result = new WorkDayCalcResult(TotalHours: 9.0, MainProjectLabel: "M", Flex: 1.0, SpecifiedHours: new() { { "M", 9.0 } });

        var formattedResult = ResultFormatter.FormatResultToLineString(result);

        Assert.That(formattedResult, Is.EqualTo("[ M 9 | Flex in 1 | Total 9 ]"));
    }

    [Test]
    public void FormatResult_FormatsNegativeFlex()
    {
        var result = new WorkDayCalcResult(TotalHours: 7.0, MainProjectLabel: "M", Flex: -1.0, SpecifiedHours: new() { { "M", 7.0 } });

        var formattedResult = ResultFormatter.FormatResultToLineString(result);

        Assert.That(formattedResult, Is.EqualTo("[ M 7 | Flex out 1 | Total 7 ]"));
    }

    [Test]
    public void FormatResult_FormatsProjects()
    {
        var result = new WorkDayCalcResult(
            TotalHours: 8.0,
            MainProjectLabel: "M",
            Flex: 0.0,
            SpecifiedHours: new() { { "M", 6.5 }, { "OtherProject", 1.5 } });

        var formattedResult = ResultFormatter.FormatResultToLineString(result);

        // Take off: find a better way to handle dotted decimals
        Assert.That(formattedResult, Is.EqualTo("[ M 6,5 | OtherProject 1,5 | Total 8 ]"));
    }

    [Test]
    public void FormatResult_FormatsProjectsAndFlex()
    {
        var result = new WorkDayCalcResult(
            TotalHours: 9.0,
            MainProjectLabel: "M",
            Flex: 1.0,
            SpecifiedHours: new() { { "M", 7.5 }, { "OtherProject", 1.5 } });

        var formattedResult = ResultFormatter.FormatResultToLineString(result);

        Assert.That(formattedResult, Is.EqualTo("[ M 7,5 | OtherProject 1,5 | Flex in 1 | Total 9 ]"));
    }
}
