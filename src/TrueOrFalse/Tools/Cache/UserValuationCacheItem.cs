using System.Collections.Generic;

public class UserValuationCacheItem
{
    public int UserId;
    public IList<CategoryValuation> CategoryValuations;
    public IList<QuestionValuation> QuestionValuations;

    public bool IsBeingRefreshed;
}