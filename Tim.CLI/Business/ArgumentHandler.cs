using System.Collections.Immutable;
using Tim.CLI.Models;

namespace Tim.CLI.Business
{
    internal static class ArgumentHandler
    {
        private const double DefaultWorkDayHours = 8.0;
        private const string WorkDayHoursFlag = "-b";
        private const string FlexFlag = "-f";

        internal static Arguments Parse(ImmutableArray<string> args)
        {
            var start = TimeOnly.Parse(args[0].Insert(2, ":"));
            var end = TimeOnly.Parse(args[1].Insert(2, ":"));
            var lunch = TimeSpan.FromHours(double.Parse(args[2].Replace(".", ",")));
            var label = args[3];

            var workDayHours = GetArgumentValueOrDefault(WorkDayHoursFlag, DefaultWorkDayHours, args);

            var flexHours = GetFlexHours(FlexFlag, args);

            return new(start, end, lunch, label, workDayHours, flexHours);

            // Later:
            // $ tim 0700 1800 0.5 MainProjectName -f 1 -f -3 --ProjectName 2.5 -b 7

            // project hours: key value list of namestring and hour
        }

        internal static List<string> Validate(ImmutableArray<string> args)
        {
            // Missing required

            // Double of unique, like base

            // Unknown parameters

            // Missing parameters (can't have two qualifiers or two datas in a row)

            // Non-valid times

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
    }
}