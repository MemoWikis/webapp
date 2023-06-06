[Serializable]
public class QuestionSetHistory : HistoryBase<QuestionSetHistoryItem>
{
}

[Serializable]
public class QuestionSetHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public HistoryItemType Type { get; set; }

    public QuestionSetHistoryItem(Set set, HistoryItemType type = HistoryItemType.Any)
    {
        Id = set.Id;
        Name = set.Name;
        Type = type;
    }
}