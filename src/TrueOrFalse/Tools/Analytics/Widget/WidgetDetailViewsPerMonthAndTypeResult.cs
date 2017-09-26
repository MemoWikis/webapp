using System;
using System.Collections.Generic;

public class WidgetDetailViewsPerMonthAndTypeResult
{
    public DateTime Month;
    public Dictionary<WidgetType, int> ViewsPerWidgetType;
}