using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class CBForAllUsersCalculator : IRegisterAsInstancePerLifetime
    {
        public int Run(IList<AnswerHistory> answerHistoryItems)
        {
            if (!answerHistoryItems.Any())
                return 30;

            decimal answeredCorrectly = answerHistoryItems.Count(x => x.AnswerredCorrectly);
            return (int)((answeredCorrectly / answerHistoryItems.Count()) * 100);
        }
    }
}
