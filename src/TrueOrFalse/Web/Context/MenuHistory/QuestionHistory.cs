using System.Web.Mvc;


[Serializable]
public class QuestionHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public string Text { get; private set; }
    public string Solution { get; private set; }
    public HistoryItemType Type { get; set; }
        
    public Func<UrlHelper, string> Link;

    public Question Question;
    public Set Set;
}