using System.Linq;

[Serializable]
public class HelpHistory : HistoryBase<HelpHistoryItem>
{
}

[Serializable]
public class HelpHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public string Text { get; private set; }
    public string Url { get; private set; }
    public HistoryItemType Type { get; set; }

    public HelpHistoryItem(string actionName)
    {
        Id = Convert.ToInt32(actionName.First()) + actionName.Length;
        Url = "/Hilfe/" + actionName;
        Text = actionName;
        Type = HistoryItemType.Any;
    }
}