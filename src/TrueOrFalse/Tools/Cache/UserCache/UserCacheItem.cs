using System.Collections.Concurrent;

public class UserCacheItem
{
    public int UserId { get; private set; }

    private User _user;
    public User User
    {
        get => _user;
        set
        {
            _user = value;
            if(value != null)
                UserId = value.Id;
        }
    }

    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations;
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations;
    public bool IsFiltered;
}