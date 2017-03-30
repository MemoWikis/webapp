public class WidgetSetStartModel : BaseModel
{
    public int SetId;
    public bool HideAddToKnowledge;

    public WidgetSetStartModel(int setId, bool hideAddToKnowledge)
    {
        SetId = setId;
        HideAddToKnowledge = hideAddToKnowledge;

        ShowUserReportWidget = false;
    }
}
