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

        public CorrectnessProbabilityModel(
            QuestionCacheItem question,
            QuestionValuationCacheItem questionValuationForUser)
        {
            UserHasAnswerHistory = questionValuationForUser.CorrectnessProbabilityAnswerCount > 0;
            QuestionHasAnswerHistory = question.TotalAnswers() > 0;

            CPPersonal = (questionValuationForUser.CorrectnessProbabilityAnswerCount == 0 ||
                          questionValuationForUser.CorrectnessProbability == -1)
                ? question.CorrectnessProbability
                : questionValuationForUser
                    .CorrectnessProbability; // instead of question.CorrectnessProbability, value should be set to average 
            // probability derived from first (!) answer of all users, once this value is saved in table

            CPAll = question.CorrectnessProbability;
            CPDerivation = Math.Abs(CPPersonal - CPAll);
            if (CPPersonal == CPAll)
                CPDerivationSign = "&plusmn;";
            else
                CPDerivationSign = (CPPersonal < CPAll) ? "-" : "+";

            switch (questionValuationForUser.KnowledgeStatus)
            {
                case KnowledgeStatus.Solid:
                    CPPColor = KnowledgeStatusColors.Solid;
                    break;
                case KnowledgeStatus.NeedsConsolidation:
                    CPPColor = KnowledgeStatusColors.NeedsConsolidation;
                    break;
                case KnowledgeStatus.NeedsLearning:
                    CPPColor = KnowledgeStatusColors.NeedsLearning;
                    break;
                default:
                    CPPColor = KnowledgeStatusColors.NotLearned;
                    break;
            }
        }
    }
}