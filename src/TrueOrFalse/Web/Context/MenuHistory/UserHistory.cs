using TrueOrFalse.Web.Uris;

namespace TrueOrFalse
{
    public class UserHistory : HistoryBase<UserHistoryItem>{}

    public class UserHistoryItem : HistoryItemBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public HistoryItemType Type { get {return HistoryItemType.Any;} }

        public UserHistoryItem(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }
    }
}
