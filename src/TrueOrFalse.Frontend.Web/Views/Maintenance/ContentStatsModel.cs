using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class ContentStatsModel : BaseModel
{
    public UIMessage Message;

    public SetViewStatsResult SetStats;

    public ContentStatsModel()
    {
        SetStats = SetViewStats.GetForId(14);
        
    }


}