using System.Collections.Generic;
using System.Linq;

public static class SetListExt
{
    public static Set GetById(this List<Set> sets, int id) => 
        sets.Find(question => question.Id == id);

    public static IList<int> GetIds(this IEnumerable<Set> sets) => 
        sets.Select(q => q.Id).ToList();
}