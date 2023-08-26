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
        var mainProjectHours = result.SpecifiedHours.First(x => x.Key == result.MainProjectLabel).Value;

        var projectsExceptMainProject = result.SpecifiedHours.Where(x => x.Key != result.MainProjectLabel);

        var formattedProjectHours =
            projectsExceptMainProject.Any() ?
            string.Join("", projectsExceptMainProject.Select(x => $"{x.Key} {x.Value} | ")) :
            string.Empty;

        var flexLabel = string.Empty;
        if (result.Flex > 0)
        {
            flexLabel = "Flex in";
        } else if (result.Flex < 0)
        {
            flexLabel = "Flex out";
        }
        var formattedFlexHours = string.IsNullOrEmpty(flexLabel) ? string.Empty : $"{flexLabel} {Math.Abs(result.Flex)} | ";

        return $"[ {result.MainProjectLabel} {mainProjectHours} | {formattedProjectHours}{formattedFlexHours}Total {result.TotalHours} ]";
    }
}
