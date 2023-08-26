namespace Tim.CLI.Models;
internal record WorkDayCalcResult(
    double TotalHours,
    double Flex,
    string MainProjectLabel,
    Dictionary<string, double> SpecifiedHours
);
