namespace Tim.CLI.Models;
internal record Arguments(TimeOnly Start, TimeOnly End, TimeSpan Lunch, string MainProjectLabel, double WorkDayHours);