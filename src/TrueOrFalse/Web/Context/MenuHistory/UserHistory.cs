using System;

[Serializable]
public class UserHistory : HistoryBase<UserHistoryItem>{}

[Serializable]
public class UserHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public HistoryItemType Type => HistoryItemType.Any;

    public UserHistoryItem(User user)
    {
        var userTiny = new UserTinyModel(user);
        Id = userTiny.Id;
        Name = userTiny.Name;
    }
}