using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse
{
    public static class TotalsPerUserExt 
    {
        public static TotalPerUser ByQuestionId(this IEnumerable<TotalPerUser> totalsPerUser, int questionId)
        {
            return totalsPerUser.FirstOrDefault(item => item.QuestionId == questionId);
        }
    }
}