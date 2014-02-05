using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse
{
    public class CorrectnessProbabilityModel
    {
        public int CP;
        public int CPDerivation;
        public string CPDerivationSign;

        public CorrectnessProbabilityModel(Question question, QuestionValuation questionValuationForUser)
        {
            CP = (questionValuationForUser.CorrectnessProbability == -1)
                ? question.CorrectnessProbability
                : questionValuationForUser.CorrectnessProbability;
            CPDerivation = CP - question.CorrectnessProbability;
            CPDerivationSign = (CP <= question.CorrectnessProbability) ? "+" : "-";
        }

    }
}