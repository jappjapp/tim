using System.ComponentModel;
using Tim.CLI.Models;

namespace Tim.CLI.Business;
internal static class WorkDayCalculator
{
    internal static WorkDayCalcResult Calculate(Arguments arguments)
    {
        var totalHours = (arguments.End - arguments.Start - arguments.Lunch + arguments.FlexHours).TotalHours;
        var flex = totalHours - arguments.WorkDayHours;
        var specifiedHours = GetProjectHours(arguments, totalHours);

        return new(totalHours, flex, specifiedHours);
    }

    internal static List<string> Validate(WorkDayCalcResult result)
    {
        // negative specifiedHours

        throw new NotImplementedException();
    }

    private static Dictionary<string, double> GetProjectHours(Arguments arguments, double totalHours)
    {
        Dictionary<string, double> specifiedHours = new();

        foreach (var project in arguments.ProjectHours)
        {
            specifiedHours[project.Key] = project.Value;
        }

        // Hours that are unaccounted for by specified projects are allocated towards the main project
        var projectSum = arguments.ProjectHours.Aggregate(0.0d, (sum, projectHour) => sum += projectHour.Value);
        var mainProjectHours = totalHours - projectSum;
        specifiedHours.Add(arguments.MainProjectLabel, mainProjectHours);

        return specifiedHours;
    }
}
