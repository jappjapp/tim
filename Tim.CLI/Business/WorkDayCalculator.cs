using System.ComponentModel;
using Tim.CLI.Models;

namespace Tim.CLI.Business;
internal static class WorkDayCalculator
{
    internal static WorkDayCalcResult Calculate(Arguments arguments)
    {
        var hoursOutsideWorkday = TimeSpan.FromHours(arguments.ProjectHoursOutsideWorkday.Sum(x => x.Value));
        var totalHours = (arguments.End - arguments.Start - arguments.Lunch + arguments.FlexHours + hoursOutsideWorkday).TotalHours;
        var flex = totalHours - arguments.WorkDayHours;
        var specifiedHours = GetProjectHours(arguments, totalHours, hoursOutsideWorkday);

        return new(totalHours, flex, specifiedHours);
    }

    internal static List<string> Validate(WorkDayCalcResult result)
    {
        // negative specifiedHours

        throw new NotImplementedException();
    }

    private static Dictionary<string, double> GetProjectHours(Arguments arguments, double totalHours, TimeSpan projectHoursOutsideWorkday)
    {
        Dictionary<string, double> specifiedHours = new();

        foreach (var projectDuringWorkday in arguments.ProjectHoursDuringWorkday)
        {
            specifiedHours[projectDuringWorkday.Key] = projectDuringWorkday.Value;
        }

        foreach (var projectOutsideWorkday in arguments.ProjectHoursOutsideWorkday)
        {
            AddOrIncrease(specifiedHours, projectOutsideWorkday.Key, projectOutsideWorkday.Value);
        }

        // Hours that are unaccounted for are allocated towards the main project
        var projectHoursDuringWorkday = arguments.ProjectHoursDuringWorkday.Sum(x => x.Value);
        var mainProjectHours = totalHours - projectHoursDuringWorkday - projectHoursOutsideWorkday.TotalHours;

        // Main project might already exist from outside workday
        AddOrIncrease(specifiedHours, arguments.MainProjectLabel, mainProjectHours);

        return specifiedHours;
    }

    private static void AddOrIncrease(Dictionary<string, double> specifiedHours, string key, double value)
    {
        if (!specifiedHours.TryAdd(key, value))
        {
            specifiedHours[key] += value;
        }
    }
}
