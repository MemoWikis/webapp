public class WidgetSetStartModel : BaseModel
{
    public int SetId;
    public string SetName;
    public string SetText;
    public bool HideAddToKnowledge;
    public string StartSessionUrl;

    public WidgetSetStartModel(int setId, bool hideAddToKnowledge)
    {
        SetId = setId;
        var set = Sl.R<SetRepo>().GetById(setId);
        SetName = set.Name;
        SetText = set.Text;

        HideAddToKnowledge = hideAddToKnowledge;

        ShowUserReportWidget = false;

        StartSessionUrl = GetStartTestSessionUrl(setId);
    }

    public static string GetStartTestSessionUrl(int setId) => $"/widget/fragesatz/{setId}";
}
