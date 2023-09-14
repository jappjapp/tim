using System.Collections.Immutable;
using System.Globalization;
using Tim.CLI.Models;

namespace Tim.CLI.Business
{
    internal static class ArgumentHandler
    {
        internal static Arguments Parse(ImmutableArray<string> args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("sv-SE"); // Use comma decimals

            var start = TimeOnly.Parse(ConvertToParsableTime(args[0]));
            var end = TimeOnly.Parse(ConvertToParsableTime(args[1]));
            var lunch = TimeSpan.FromHours(double.Parse(args[2]));
            var label = args[3];

            var workDayHours = GetArgumentValueOrDefault(Constants.WorkDayHoursFlag, Constants.DefaultWorkDayHours, args);

            var flexHours = GetFlexHours(Constants.FlexFlag, args);

            var projectHoursDuringWorkday = GetProjectHours(Constants.ProjectsDuringWorkdayFlagPrefix, args);
            var projectHoursOutsideWorkday = GetProjectHours(Constants.ProjectsOutsideWorkdayFlagPrefix, args);

            return new(start, end, lunch, label, workDayHours, flexHours, projectHoursDuringWorkday, projectHoursOutsideWorkday);
        }

        internal static bool IsHelpArgument(ImmutableArray<string> immutableArgs)
        {
            return immutableArgs.Length == 1 && immutableArgs[0].Equals(Constants.HelpArgument);
        }

        internal static List<string> Validate(ImmutableArray<string> args)
        {
            var errors = new List<string>();

            // Missing required args
            if (args.Length < Constants.NumberRequiredArgs)
            {
                errors.Add(Constants.ValidationError_TooFewArguments);
            }

            // Start time
            if (!TimeOnly.TryParse(ConvertToParsableTime(args[0]), out TimeOnly _))
            {
                errors.Add(Constants.ValidationError_MalformattedStartTime);
            }

            // End time
            if (!TimeOnly.TryParse(ConvertToParsableTime(args[1]), out TimeOnly _))
            {
                errors.Add(Constants.ValidationError_MalformattedEndTime);
            }

            // Lunch duration
            if (!double.TryParse(args[2], out double _))
            {
                errors.Add(Constants.ValidationError_MalformattedLunchDuration);
            }

            // More than one unique flag
            if (args.Count(x => x.Equals(Constants.WorkDayHoursFlag)) > 1)
            {
                errors.Add(Constants.ValidationError_MoreThanOneWorkdayFlag);
            }

            // Non-required args, validate syntax and data
            var argValuePairs =
                args.Length > Constants.NumberRequiredArgs ?
                args.Skip(Constants.NumberRequiredArgs).ToList() :
                new List<string>();

            var hasEvenNumberNonRequiredArgs = argValuePairs.Count % 2 == 0;

            if (!hasEvenNumberNonRequiredArgs)
            {
                errors.Add(Constants.ValidationError_InvalidNumberNonRequiredArgs);
            }

            while (hasEvenNumberNonRequiredArgs && argValuePairs.Count > 0)
            {
                var argValuePair = argValuePairs.Take(2).ToArray();
                var arg = argValuePair[0];
                var value = argValuePair[1];

                if (!IsValidFlag(arg))
                {
                    errors.Add($"Argument '{arg}' does not seem to be a valid flag.");
                    break; // Let user fix errors one at a time
                }

                if (!IsValidDuration(value))
                {
                    errors.Add($"Argument '{arg}' does not seem to have a valid value: '{value}'.");
                    break; // Let user fix errors one at a time
                }

                argValuePairs.RemoveRange(0, 2);
            }

            return errors;
        }

        private static bool IsValidDuration(string argValue)
        {
            return double.TryParse(argValue, out double _);
        }

        private static bool IsValidFlag(string arg)
        {
            if (Constants.FlagPrefixes.Any(x => arg.StartsWith(x)) && arg.Length > 3)
            {
                return true;
            }

            if (Constants.Flags.Contains(arg))
            {
                return true;
            }

            return false;
        }

        private static double GetArgumentValueOrDefault(string argumentFlag, double defaultValue, ImmutableArray<string> arguments)
        {
            if (!arguments.Contains(argumentFlag))
            {
                return defaultValue;
            }

            string unparsedValue = arguments[arguments.IndexOf(argumentFlag) + 1];

            return double.Parse(unparsedValue);
        }
        
        private static TimeSpan GetFlexHours(string flexFlag, ImmutableArray<string> args)
        {
            var flexHours = 0.0;
            var mutableArgs = args.ToList();

            while (mutableArgs.Contains(flexFlag))
            {
                var flexFlagIndex = mutableArgs.IndexOf(flexFlag);

                flexHours += double.Parse(mutableArgs[flexFlagIndex + 1]);

                mutableArgs.RemoveRange(flexFlagIndex, 2);
            }

            return TimeSpan.FromHours(flexHours);
        }
        
        private static Dictionary<string, double> GetProjectHours(string projectFlagPrefix, ImmutableArray<string> args)
        {
            Dictionary<string, double> projectHours = new();
            var mutableArgs = args.ToList();

            while (mutableArgs.Any(a => a.StartsWith(projectFlagPrefix)))
            {
                var projectIndex = mutableArgs.IndexOf(mutableArgs.First(a => a.StartsWith(projectFlagPrefix)));
                var projectName = mutableArgs[projectIndex].Remove(0, projectFlagPrefix.Length);
                var projectValue = double.Parse(mutableArgs[projectIndex + 1]);

                if (!projectHours.TryAdd(projectName, projectValue))
                {
                    projectHours[projectName] += projectValue;
                }

                mutableArgs.RemoveRange(projectIndex, 2);
            }

            return projectHours;
        }

        private static string ConvertToParsableTime(string arg)
        {
            return arg.Insert(2, ":");
        }
    }
}