using System.Collections.Immutable;
using Tim.CLI.Business;

namespace Tim.CLI.Tests;

public class ArgumentHandlerTests
{
    [Test]
    public void ParseAndValidate_HandlesSimplifiedInputFormat()
    {
        var args = new ImmutableArray<string> { "0800", "1700", "0.5", "Label" };

        var arguments = ArgumentHandler.Parse(args);

        Assert.Multiple(() =>
        {
            Assert.That(arguments.Start, Is.EqualTo(TimeOnly.FromTimeSpan(TimeSpan.FromHours(8))));
            Assert.That(arguments.End, Is.EqualTo(TimeOnly.FromTimeSpan(TimeSpan.FromHours(17))));
            Assert.That(arguments.Lunch, Is.EqualTo(TimeSpan.FromMinutes(30)));
            Assert.That(arguments.MainProjectLabel, Is.EqualTo("Label"));
        });
    }
}
