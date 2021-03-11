using System;

namespace TrueOrFalse
{
    public class CorrectnessProbabilityModel
    {
        public int CPPersonal;
        public bool UserHasAnswerHistory;
        public bool QuestionHasAnswerHistory;
        public int CPAll; 
        public int CPDerivation;
        public string CPDerivationSign;
        public string CPPColor;

        public CorrectnessProbabilityModel(Question question, QuestionValuationCacheItem questionValuationForUser)
        {
            UserHasAnswerHistory = questionValuationForUser.CorrectnessProbabilityAnswerCount > 0;
            QuestionHasAnswerHistory = question.TotalAnswers() > 0;

            CPPersonal = (questionValuationForUser.CorrectnessProbabilityAnswerCount == 0 || questionValuationForUser.CorrectnessProbability == -1)
                ? question.CorrectnessProbability
                : questionValuationForUser.CorrectnessProbability;  // instead of question.CorrectnessProbability, value should be set to average 
                                                                    // probability derived from first (!) answer of all users, once this value is saved in table

            CPAll = question.CorrectnessProbability;
            CPDerivation = Math.Abs(CPPersonal - CPAll);
            if (CPPersonal == CPAll)
                CPDerivationSign = "&plusmn;";
            else
                CPDerivationSign = (CPPersonal < CPAll) ? "-" : "+";

            if (CPPersonal >= 80)
                CPPColor = "#AFD534";
            else if (CPPersonal < 80 && CPPersonal >= 50)
                CPPColor = "#FDD648";
            else if (CPPersonal < 50 && CPPersonal >= 0)
                CPPColor = "#FFA07A";
            else
                CPPColor = "#949494";
        }

    }
}