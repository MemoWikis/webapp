using System.Collections.Concurrent;
using System.Collections.Generic;

public class UserValuationCacheItem
{
    public int UserId;
    
    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations;
    public ConcurrentDictionary<int, QuestionValuation> QuestionValuations;
}