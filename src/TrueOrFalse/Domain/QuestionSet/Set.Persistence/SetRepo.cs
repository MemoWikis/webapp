using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

public class SetRepo : RepositoryDbBase<Set>
{
    public SetRepo(
        ISession session)
        : base(session)
    {
    }

    public IList<Set> GetByIds(List<int> setIds)
    {
        return GetByIds(setIds.ToArray());
    }

    public override IList<Set> GetByIds(params int[] setIds)
    {
        var resultTmp = base.GetByIds(setIds);
        var result = new List<Set>();
        for (int i = 0; i < setIds.Length; i++)
        {
            if (resultTmp.Any(c => c.Id == setIds[i]))
                result.Add(resultTmp.First(c => c.Id == setIds[i]));
        }
        return result;
    }

    public int HowManyNewSetsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Set>()
            .Where(s => s.DateCreated > since)
            .RowCount();
    }

    public int TotalSetCount()
    {
        return _session.QueryOver<Set>()
            .RowCount();
    }

    public string GetYoutbeUrl(int setId) => 
        _session.Query<Set>()
            .Where(s => s.Id == setId)
            .Select(s => s.VideoUrl)
            .FirstOrDefault();
}