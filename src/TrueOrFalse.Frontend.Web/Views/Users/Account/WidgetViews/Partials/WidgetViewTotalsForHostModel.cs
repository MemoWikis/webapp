using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class WidgetViewTotalsForHostModel : BaseModel
{

    public string Host;
    public string HostOnlyAlphaNumerical;
    public IList<WidgetViewsPerMonthAndKeyResult> WidgetViewsPerMonthAndKeyResults;
    public IList<string> WidgetKeys;

    public WidgetViewTotalsForHostModel(string host)
    {
        Host = host;
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        HostOnlyAlphaNumerical = rgx.Replace(host, "_");
        WidgetKeys = Sl.R<WidgetViewRepo>().GetWidgetKeys(host).OrderBy(k => k).ToList();
        WidgetViewsPerMonthAndKeyResults = Sl.R<WidgetViewRepo>().GetWidgetViewsPerMonthAndKeyResults(host);

    }
}
