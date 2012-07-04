using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Core
{
    public static class TotalsPerUserExt 
    {
        public static TotalPerUser ByQuestionId(this IEnumerable<TotalPerUser> totalsPerUser, int questionId)
        {
            return totalsPerUser.First(item => item.QuestionId == questionId);
        }
    }
}