using TrueOrFalse;


[Serializable]
public class CategoryHistory : HistoryBase<CategoryHistoryItem>
{
}

[Serializable]
public class CategoryHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public HistoryItemType Type { get; set; }
}

