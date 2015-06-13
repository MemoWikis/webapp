
public enum HistoryItemType{ Any, Edit}

public interface HistoryItemBase
{
    int Id { get;}
    HistoryItemType Type { get; }
}