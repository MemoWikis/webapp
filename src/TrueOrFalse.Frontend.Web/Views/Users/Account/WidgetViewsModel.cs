using System.Collections.Generic;
using System.Linq;

public class WidgetViewsModel : BaseModel
{
    public IList<WidgetViewsPerMonthAndKeyResult> WidgetViewsPerMonthAndKeyResults;
    public IList<string> WidgetKeys;

    public WidgetViewsModel()
    {
        WidgetViewsPerMonthAndKeyResults = Sl.R<WidgetViewRepo>().GetWidgetViewsPerMonthAndKeyResults("www.beltz.de");
        WidgetKeys = Sl.R<WidgetViewRepo>().GetWidgetKeys("www.beltz.de").OrderBy(k => k).ToList();
    }

}