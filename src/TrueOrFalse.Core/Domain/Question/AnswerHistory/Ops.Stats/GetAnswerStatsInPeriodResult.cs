using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class GetAnswerStatsInPeriodResult
    {
        public int TotalAnswers;
        public int TotalTrueAnswers;
        public int TotalFalseAnswers { get { return TotalAnswers - TotalTrueAnswers; } }
    }
}
