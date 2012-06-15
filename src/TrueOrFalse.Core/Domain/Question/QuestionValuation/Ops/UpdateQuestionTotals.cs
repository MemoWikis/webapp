using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class UpdateQuestionTotals : IRegisterAsInstancePerLifetime
    {
        public void Run(QuestionValuation questionValuation)
        {
            questionValuation.IsSetQuality()
        }

    }
}
