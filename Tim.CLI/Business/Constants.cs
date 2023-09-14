namespace Tim.CLI.Business;
public static class Constants
{
    public const int NumberRequiredArgs = 4;

    // Default values
    public const double DefaultWorkDayHours = 8.0;

    // Flag prefixes and arguments
    public const string WorkDayHoursFlag = "-b";
    public const string FlexFlag = "-f";
    public const string ProjectsDuringWorkdayFlagPrefix = "--";
    public const string ProjectsOutsideWorkdayFlagPrefix = "++";
    public const string HelpArgument = "--help";

    public static string[] FlagPrefixes { get; } = {
        ProjectsDuringWorkdayFlagPrefix,
        ProjectsOutsideWorkdayFlagPrefix,
    };

    public static string[] Flags { get; } = {
        WorkDayHoursFlag,
        FlexFlag,
    };


    // Validation error texts
    public const string ValidationError_TooFewArguments = $"To few arguments. Pass '{HelpArgument}' to learn more.";
    public const string ValidationError_MalformattedLunchDuration = "Lunch duration seems malformatted. Should be a decimal number with comma.";
    public const string ValidationError_MalformattedStartTime = "Start time seems malformatted. Should be like '0800' for 08:00";
    public const string ValidationError_MalformattedEndTime = "End time seems malformatted. Should be like '1700' for 17:00";
    public const string ValidationError_MoreThanOneWorkdayFlag = $"More than one flag for custom workday length ({WorkDayHoursFlag})";
    public const string ValidationError_WhitespaceInArgFlags = "There seems to be whitespace one or more flags, like '-- MyLabel' instead of '--MyLabel'";
    public const string ValidationError_InvalidNumberNonRequiredArgs = "There seems to be a flag or a data value missing among the non-required arguments";

}
