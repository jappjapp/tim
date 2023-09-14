using System.Collections.Immutable;
using Tim.CLI.Business;

namespace Tim.CLI.Tests;
public class ArgumentValidationTests
{
    [Test]
    public void Validate_ValidArgsYieldNoErrors()
    {
        var args = new[] { "0800", "1700", "1", "MainProject" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(0));
    }

    [Test]
    public void Validate_TooFewArgsYieldError()
    {
        var args = new[] { "0800", "1700", "1"}.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_TooFewArguments));
    }

    [Test]
    public void Validate_MalformattedStartTimeYieldsError()
    {
        var args = new[] { "abc", "1700", "1", "MainProject" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_MalformattedStartTime));
    }

    [Test]
    public void Validate_MalformattedEndTimeYieldsError()
    {
        var args = new[] { "0800", "abc", "1", "MainProject" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_MalformattedEndTime));
    }

    [Test]
    public void Validate_NonNumericalLunchDurationYieldsError()
    {
        var args = new[] { "0800", "1700", "abc", "MainProject" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_MalformattedLunchDuration));
    }

    [Test]
    public void Validate_DottedDecimalLunchDurationYieldsError()
    {
        var args = new[] { "0800", "1700", "1.5", "MainProject" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_MalformattedLunchDuration));
    }

    [Test]
    public void Validate_MoreThanOneWordDayHoursFlagYieldsError()
    {
        var args = new[] { "0800", "1700", "1", "MainProject", "-b", "7", "-b", "7" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_MoreThanOneWorkdayFlag));
    }

    [Test]
    public void Validate_MissingNonRequiredArgValueYieldsError()
    {
        var args = new[] { "0800", "1700", "1", "MainProject", "--Test" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(Constants.ValidationError_InvalidNumberNonRequiredArgs));
    }

    [Test]
    public void Validate_NonValidFlag1YieldsError()
    {
        var args = new[] { "0800", "1700", "1", "MainProject", Constants.ProjectsDuringWorkdayFlagPrefix, "2" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Does.Contain(Constants.ProjectsDuringWorkdayFlagPrefix));
    }

    [Test]
    public void Validate_NonValidFlag2YieldsError()
    {
        var args = new[] { "0800", "1700", "1", "MainProject", Constants.ProjectsOutsideWorkdayFlagPrefix, "2" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Does.Contain(Constants.ProjectsOutsideWorkdayFlagPrefix));
    }

    [Test]
    public void Validate_NonValidArgValueYieldsError()
    {
        var args = new[] { "0800", "1700", "1", "MainProject", "--Test", "abc" }.ToImmutableArray();

        var errors = ArgumentHandler.Validate(args);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors.First(), Does.Contain("--Test"));
        Assert.That(errors.First(), Does.Contain("abc"));
    }


}
