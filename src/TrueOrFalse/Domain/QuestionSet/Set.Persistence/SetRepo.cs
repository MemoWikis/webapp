using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class SetRepo : RepositoryDbBase<Set>
{
    public SetRepo(
        ISession session)
        : base(session)
    {
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

    public string GetYoutbeUrl(int setId) => 
        _session.Query<Set>()
            .Where(s => s.Id == setId)
            .Select(s => s.VideoUrl)
            .FirstOrDefault();
}