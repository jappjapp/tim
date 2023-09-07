using System.Globalization;
using Tim.CLI.Models;

namespace Tim.CLI.Business;
internal static class ResultFormatter
{
    internal static string FormatErrorsToLineString(List<string> errors)
    {
        throw new NotImplementedException();
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

    private static double Round(double value)
    {
        return Math.Round(value, 2);
    }
}
