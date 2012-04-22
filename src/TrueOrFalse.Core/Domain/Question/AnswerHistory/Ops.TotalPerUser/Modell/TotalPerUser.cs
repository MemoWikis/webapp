using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class TotalPerUser
    {
        public int QuestionId;
        public int TotalTrue;
        public int TotalFalse;

        public int Total(){ return TotalTrue + TotalFalse;}
        public int PercentageTrue(){ return 0;}
    }

}
