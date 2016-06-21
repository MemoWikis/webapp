using System;

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
            CPPersonal = (questionValuationForUser.CorrectnessProbabilityAnswerCount == 0 || questionValuationForUser == null || questionValuationForUser.CorrectnessProbability == -1)
                ? question.CorrectnessProbability
                : questionValuationForUser.CorrectnessProbability;
            CPAll = question.CorrectnessProbability;
            CPDerivation = Math.Abs(CPPersonal - CPAll);
            if (CPPersonal == CPAll)
                CPDerivationSign = "&plusmn;";
            else
                CPDerivationSign = (CPPersonal < CPAll) ? "-" : "+";
        }

    }
}