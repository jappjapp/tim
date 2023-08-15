using System.Collections.Immutable;
using Tim.CLI.Business;

namespace Tim.CLI;
public class Program
{
    static void Main(string[] args)
    {
        var immutableArgs = args.ToImmutableArray();

        var errors = ArgumentHandler.Validate(immutableArgs);
        if (errors.Any())
        {
            Console.WriteLine(ResultFormatter.FormatErrorsToLineString(errors));
            return;
        }

        var parsedArgs = ArgumentHandler.Parse(immutableArgs);

        var result = WorkDayCalculator.Calculate(parsedArgs);

        var resultErrors = WorkDayCalculator.Validate(result);
        if (resultErrors.Any())
        {
            Console.WriteLine(ResultFormatter.FormatErrorsToLineString(resultErrors));
            return;
        }

        var formattedResult = ResultFormatter.FormatResultToLineString(result);

        Console.WriteLine(formattedResult);
    }
}

