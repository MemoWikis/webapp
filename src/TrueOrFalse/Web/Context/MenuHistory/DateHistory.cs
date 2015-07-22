using System;
[Serializable]
public class DateHistory : HistoryBase<DateHistoryItem>
{
}

[Serializable]
public class DateHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public HistoryItemType Type { get; set; }

    public DateHistoryItem(
        Date date, 
        HistoryItemType type = HistoryItemType.Any)
    {
        Id = date.Id;
        Name = date.GetInfo();
        Type = type;
    }
}