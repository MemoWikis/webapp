using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse
{
    public class CorrectnessProbabilityModel
    {
        public int CPPersonal;
        public int CPAll; 
        public int CPDerivation;
        public string CPDerivationSign;

        public CorrectnessProbabilityModel(Question question, QuestionValuation questionValuationForUser)
        {
            questionValuationForUser = questionValuationForUser ?? new QuestionValuation();

            CPPersonal = (questionValuationForUser.CorrectnessProbability == -1)
                ? question.CorrectnessProbability
                : questionValuationForUser.CorrectnessProbability;
            CPAll = question.CorrectnessProbability;
            CPDerivation = Math.Abs(CPPersonal - question.CorrectnessProbability);
            CPDerivationSign = (CPPersonal <= question.CorrectnessProbability) ? "-" : "+";
        }

    }
}