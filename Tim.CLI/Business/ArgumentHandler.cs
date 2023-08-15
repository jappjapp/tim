using System.Collections.Immutable;
using Tim.CLI.Models;

namespace Tim.CLI.Business
{
    internal static class ArgumentHandler
    {
        private const double DefaultWorkDayHours = 8.0;
        private const string WorkDayHoursFlag = "-b";
        private const string FlexFlag = "-f";
        private const string ProjectFlagPrefix = "--";

        internal static Arguments Parse(ImmutableArray<string> args)
        {
            var start = TimeOnly.Parse(args[0].Insert(2, ":"));
            var end = TimeOnly.Parse(args[1].Insert(2, ":"));
            var lunch = TimeSpan.FromHours(double.Parse(args[2].Replace(".", ",")));
            var label = args[3];

            var workDayHours = GetArgumentValueOrDefault(WorkDayHoursFlag, DefaultWorkDayHours, args);

            var flexHours = GetFlexHours(FlexFlag, args);

            var projectHours = GetProjectHours(ProjectFlagPrefix, args);

            return new(start, end, lunch, label, workDayHours, flexHours, projectHours);
        }

        internal static List<string> Validate(ImmutableArray<string> args)
        {
            // Missing required

            // Double of unique, like base

            // Unknown parameters

            // Missing parameters (can't have two qualifiers or two datas in a row)

            // Non-valid times

            // only double dash, no project name

            throw new NotImplementedException();
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

                flexHours += double.Parse(mutableArgs[flexFlagIndex + 1].Replace(".", ","));

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
                var projectValue = double.Parse(mutableArgs[projectIndex + 1].Replace(".", ","));

                if (!projectHours.TryAdd(projectName, projectValue))
                {
                    projectHours[projectName] += projectValue;
                }

                mutableArgs.RemoveRange(projectIndex, 2);
            }

            return projectHours;

        }
    }
}