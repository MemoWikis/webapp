using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

public class AlgoInsightModel : BaseModel
{
    public UIMessage Message;
    public IEnumerable<AlgoTesterSummary> Summaries;

    public AlgoInsightModel()
    {
        Summaries = AlgoTesterSummaryLoader.Run().OrderByDescending(x => x.SuccessRate);
    }
}