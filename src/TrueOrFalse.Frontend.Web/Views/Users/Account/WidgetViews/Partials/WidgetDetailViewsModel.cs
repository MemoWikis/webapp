using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class WidgetDetailViewsModel : BaseModel
{
    public string HostOnlyAlphaNumerical;

    public IList<WidgetDetailViewsPerMonthAndTypeResult> WidgetDetailViewsPerMonthAndType;
    public IList<WidgetType> WidgetTypes;


    public WidgetDetailViewsModel(string host, string widgetKey)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        HostOnlyAlphaNumerical = rgx.Replace(host, "_");
        WidgetDetailViewsPerMonthAndType = Sl.R<WidgetViewRepo>().GetWidgetDetailViewsPerMonthAndType(host, widgetKey);
        WidgetTypes = Sl.R<WidgetViewRepo>().GetWidgetTypes(host, widgetKey);

    }
}
