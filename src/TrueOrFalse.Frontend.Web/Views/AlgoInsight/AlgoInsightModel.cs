using System.Collections.Generic;
using TrueOrFalse.Web;

public class AlgoInsightModel : BaseModel
{
    public UIMessage Message;
    public List<AlgoTesterSummary> Summaries;

    public AlgoInsightModel()
    {
        Summaries = AlgoTesterSummaryLoader.Run();
    }
}