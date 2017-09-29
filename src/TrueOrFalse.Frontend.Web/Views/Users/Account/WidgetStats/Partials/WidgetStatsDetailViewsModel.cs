using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class WidgetStatsDetailViewsModel : BaseModel
{
    public string HostOnlyAlphaNumerical;
    public string WidgetKey;
    public string Host;

    public IList<WidgetDetailViewsPerMonthAndTypeResult> WidgetDetailViewsPerMonthAndType;
    public IList<WidgetType> WidgetTypes;


    public WidgetStatsDetailViewsModel(string host, string widgetKey)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        HostOnlyAlphaNumerical = rgx.Replace(host, "_");
        Host = host;
        WidgetKey = widgetKey;
        WidgetDetailViewsPerMonthAndType = Sl.R<WidgetViewRepo>().GetWidgetDetailViewsPerMonthAndType(host, widgetKey);
        WidgetTypes = Sl.R<WidgetViewRepo>().GetWidgetTypes(host, widgetKey);

    }
}
