using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

public class ContentStatsModel : BaseModel
{
    public UIMessage Message;

    public IList<SetViewStatsResult> SetStats;

    public ContentStatsModel(bool recalcStats = false)
    {
        if (recalcStats)
        {
            SetStats = Sl.R<SetRepo>()
                .Query
                .List()
                .Select(s => SetViewStats.GetForId(s.Id))
                .OrderByDescending(s => s.QuestionViewsDailyAvg)
                .ToList();
        }
        else
        {
            SetStats = new List<SetViewStatsResult>();
        }

    }


}