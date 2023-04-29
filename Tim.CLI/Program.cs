namespace Tim.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var (start, end, lunch, mainProjectLabel) = Arguments.ParseAndValidate(args);
            var (mainTime, mainLabel) = WorkDay.Calculate(start, end, lunch, mainProjectLabel);
            var result = Result.Format(mainTime, mainLabel);

            Console.WriteLine(result);
        }
    }

    public static class Arguments
    {
        public static (TimeOnly start, TimeOnly end, TimeSpan lunch, string mainProjectLabel) ParseAndValidate(string[] args)
        {
            return (TimeOnly.MaxValue, TimeOnly.MaxValue, TimeSpan.FromHours(1), "projlabelmock");
        }
    }

    public static class Result
    {
        public static string Format(TimeSpan mainTime, string mainLabel)
        {
            return mainLabel;
        }
    }

    public static class WorkDay
    {
        public static (TimeSpan mainTime, string mainProjectLabel) Calculate(
            TimeOnly start,
            TimeOnly end,
            TimeSpan lunch,
            string mainProjectLabel)
        {
            return (TimeSpan.FromHours(2), mainProjectLabel);
        }

    }
}

