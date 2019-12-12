using System.Collections.Concurrent;

public class UserCacheItem
{
    public int UserId;
    
    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations;
    public ConcurrentDictionary<int, QuestionValuation> QuestionValuations;
    public ConcurrentDictionary<int, SetValuation> SetValuations;
}