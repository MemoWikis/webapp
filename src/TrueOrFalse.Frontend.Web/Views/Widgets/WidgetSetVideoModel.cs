public class WidgetSetVideoModel : BaseModel
{
    public string MetaTitle => Set.Name;
    public string MetaDescription => SeoUtils.ReplaceDoubleQuotes(Set.Text).Truncate(250, true);

    public int SetId => Set.Id;
    public string SetName => Set.Name;

    public readonly Set Set;

    public bool HideAddToKnowledge;

    public WidgetSetVideoModel(Set set, bool hideAddToKnowledge = false)
    {
        ShowUserReportWidget = false;
        HideAddToKnowledge = hideAddToKnowledge;
        Set = set;
    }
}