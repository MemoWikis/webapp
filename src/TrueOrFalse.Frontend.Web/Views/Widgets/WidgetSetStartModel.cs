public class WidgetSetStartModel : WidgetBaseModel
{
    public int SetId;
    public string SetName;
    public string SetText;
    public bool HideAddToKnowledge;
    public string StartSessionUrl;

    public WidgetSetStartModel(int setId, bool hideAddToKnowledge, string host, int questionCount) : base(host)
    {
        SetId = setId;
        var set = Sl.R<SetRepo>().GetById(setId);
        SetName = set.Name;
        SetText = set.Text;

        HideAddToKnowledge = hideAddToKnowledge;

        ShowUserReportWidget = false;

        StartSessionUrl = GetStartTestSessionUrl(setId, hideAddToKnowledge, host, questionCount);
    }

    public static string GetStartTestSessionUrl(int setId, bool hideAddToKnowledge, string host, int questionCount) 
        => $"/widget/fragesatz/{setId}?hideAddToKnowledge={hideAddToKnowledge}&host={host}";
}
