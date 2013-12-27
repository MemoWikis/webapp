using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class ReputationCalcResult
    {
        public User User;

        public int ForQuestionsCreated;
        public int ForQuestionsWishCount;
        public int ForQuestionsWishKnow;

        public int ForSetWishCount;
        public int ForSetWishKnow;

        public int TotalRepuation
        {
            get
            {
                return
                    ForQuestionsCreated +
                    ForQuestionsWishCount +
                    ForQuestionsWishKnow +
                    ForSetWishCount +
                    ForSetWishKnow;
            }
        }
    }
}
