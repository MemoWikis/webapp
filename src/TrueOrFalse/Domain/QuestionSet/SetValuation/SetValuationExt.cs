using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public static class SetValuationExt
    {
        public static SetValuation BySetId(this IEnumerable<SetValuation> questionValuations, int setId)
        {
            return questionValuations.FirstOrDefault(x =>  x.SetId == setId);
        }
    }
}
