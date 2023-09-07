using Tim.CLI.Business;
using Tim.CLI.Models;

namespace Tim.CLI.Tests;
public class ResultValidationTests
{
    [Test]
    public void ResultValidation_CatchesNegativeTotalHours()
    {
        var result = new WorkDayCalcResult(TotalHours: -2, Flex: 0, MainProjectLabel: "Main", new() { { "Main", 8 } });

        var validationErrors = WorkDayCalculator.Validate(result);

        Assert.That(validationErrors, Has.Count.EqualTo(1));
        Assert.That(validationErrors[0], Is.EqualTo("Total hours are negative: -2"));
    }

    [Test]
    public void ResultValidation_CatchesNegativeProjectHours()
    {
        var result = new WorkDayCalcResult(TotalHours: 8, Flex: 0, MainProjectLabel: "Main", new() { { "Main", 3 }, { "Project 1", -2 }, { "Project 2", -3 } });

        var validationErrors = WorkDayCalculator.Validate(result);

        Assert.That(validationErrors, Has.Count.EqualTo(2));
        Assert.That(validationErrors[0], Is.EqualTo("Project 1 has negative value: -2"));
        Assert.That(validationErrors[1], Is.EqualTo("Project 2 has negative value: -3"));
    }


}
