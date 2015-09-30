using TrueOrFalse.Web;

public class AlgoInsightModel : BaseModel
{
    public UIMessage Message;

    public bool IsActiveTabForecast;
    public bool IsActiveTabForgettingCurve;
    public bool IsActiveTabRepetition;
    public bool IsActiveTabVarious;

    public AlgoInsightModel()
    {
    }
}