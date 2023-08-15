namespace Tim.CLI.Models;
internal record WorkDayCalcResult(
    double TotalHours,
    double Flex, 
    Dictionary<string, double> SpecifiedHours
);
