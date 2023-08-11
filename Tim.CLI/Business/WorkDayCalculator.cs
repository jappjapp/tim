﻿using Tim.CLI.Models;

namespace Tim.CLI.Business;
internal static class WorkDayCalculator
{
    internal static WorkDayCalcResult Calculate(Arguments arguments)
    {
        var totalHours = (arguments.End - arguments.Start - arguments.Lunch + arguments.FlexHours).TotalHours;
        var flex = totalHours - arguments.WorkDayHours;

        return new(totalHours, arguments.MainProjectLabel, flex);
    }

}