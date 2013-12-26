using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class ReputationCalcResult
    {
        public int ForQuestionsCreated;
        public int ForQuestionsWishCount;
        public int ForQuestionsWishMemory;

        public int TotalRepuation
        {
            get
            {
                return
                    ForQuestionsCreated +
                    ForQuestionsWishCount +
                    ForQuestionsWishMemory;
            }
        }
    }
}
