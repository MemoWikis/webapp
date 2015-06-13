using System.Collections.Generic;
using System.Linq;

public static class SetValuationExt
{
    public static SetValuation BySetId(this IEnumerable<SetValuation> setValuations, int setId)
    {
        return setValuations.FirstOrDefault(x =>  x.SetId == setId);
    }

    public static IList<int> SetIds(this IEnumerable<SetValuation> setValuations)
    {
        return setValuations.Select(x => x.SetId).ToList();
    }
}