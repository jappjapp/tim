﻿using System.Collections.Immutable;
using System.Globalization;
using Tim.CLI.Models;

namespace Tim.CLI.Business
{
    internal static class ArgumentHandler
    {
        private const double DefaultWorkDayHours = 8.0;
        private const string WorkDayHoursFlag = "-b";
        private const string FlexFlag = "-f";
        private const string ProjectsDuringWorkdayFlagPrefix = "--";
        private const string ProjectsOutsideWorkdayFlagPrefix = "++";

        internal static Arguments Parse(ImmutableArray<string> args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("sv-SE"); // Use comma decimals

            var start = TimeOnly.Parse(args[0].Insert(2, ":"));
            var end = TimeOnly.Parse(args[1].Insert(2, ":"));
            var lunch = TimeSpan.FromHours(double.Parse(args[2]));
            var label = args[3];

            var workDayHours = GetArgumentValueOrDefault(WorkDayHoursFlag, DefaultWorkDayHours, args);

            var flexHours = GetFlexHours(FlexFlag, args);

            var projectHoursDuringWorkday = GetProjectHours(ProjectsDuringWorkdayFlagPrefix, args);
            var projectHoursOutsideWorkday = GetProjectHours(ProjectsOutsideWorkdayFlagPrefix, args);

            return new(start, end, lunch, label, workDayHours, flexHours, projectHoursDuringWorkday, projectHoursOutsideWorkday);
        }

        internal static bool IsHelpArgument(ImmutableArray<string> immutableArgs)
        {
            return immutableArgs.Length == 1 && immutableArgs[0].Equals(Constants.HelpArgument);
        }

        internal static List<string> Validate(ImmutableArray<string> args)
        {
            // Missing required

            // Double of unique, like base

            // Unknown parameters

            // Missing parameters (can't have two qualifiers or two datas in a row)

            // Non-valid times

            // only double dash, no project name

            // No projects during workday with mainprojectlabel

            return new();
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
    }
}