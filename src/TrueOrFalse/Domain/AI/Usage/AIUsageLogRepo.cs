using NHibernate;
public class AiUsageLogRepo(ISession _session) : RepositoryDbBase<AiUsageLog>(_session)
{
    public void AddUsage(int userId, int pageId, int tokenIn, int tokenOut, string model)
    {
        var aiUsageLog = new AiUsageLog
        {
            UserId = userId,
            PageId = pageId,
            TokenIn = tokenIn,
            TokenOut = tokenOut,
            DateCreated = DateTime.UtcNow,
            Model = model
        };

        base.Create(aiUsageLog);
    }
}