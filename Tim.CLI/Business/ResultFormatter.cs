using System.Globalization;
using System.Text;
using Tim.CLI.Models;

namespace Tim.CLI.Business;
internal static class ResultFormatter
{
    internal static string FormatErrorsToLineString(List<string> errors)
    {
        StringBuilder sb = new();

        sb.AppendLine("ERRORS:");

        errors.ForEach(e => sb.AppendLine(e));

        return sb.ToString();
    }

    internal static string FormatResultToLineString(WorkDayCalcResult result)
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US"); // Use dotted decimals in output

        var mainProjectHours = result.SpecifiedHours.First(x => x.Key == result.MainProjectLabel).Value;

        var projectsExceptMainProject = result.SpecifiedHours.Where(x => x.Key != result.MainProjectLabel);

        var formattedProjectHours =
            projectsExceptMainProject.Any() ?
            string.Join("", projectsExceptMainProject.Select(x => $"{x.Key} {Round(x.Value)} | ")) :
            string.Empty;

        var flexLabel = string.Empty;
        if (result.Flex > 0)
        {
            flexLabel = "Flex in";
        } else if (result.Flex < 0)
        {
            flexLabel = "Flex out";
        }
        var formattedFlexHours = string.IsNullOrEmpty(flexLabel) ? string.Empty : $"{flexLabel} {Math.Abs(Round(result.Flex))} | ";

        mainProjectHours = Round(mainProjectHours);
        var totalHours = Round(result.TotalHours);

        return $"[ {result.MainProjectLabel} {mainProjectHours} | {formattedProjectHours}{formattedFlexHours}Total {totalHours} ]";
    }

    internal static string GetHelpText()
    {
        var sb = new StringBuilder();

        sb.AppendLine("HELP:");
        sb.AppendLine("Required args: 0800 1700 0,5 MainProject (start time, end time, lunch duration, project label)");
        sb.AppendLine("Flex in: -f 1");
        sb.AppendLine("Flex out: -f -2,5");
        sb.AppendLine("Custom workday hours: -b 7,0");
        sb.AppendLine("Project hours during workday: --ProjectLabel 3,5");
        sb.AppendLine("Project hours after workday: ++ProjectLabel 2");

        return sb.ToString();
    }

    private static double Round(double value)
    {
        return Math.Round(value, 2);
    }
}
