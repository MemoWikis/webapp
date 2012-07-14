namespace TrueOrFalse.Core
{
    public class NotNull
    {
        public static QuestionValuation Run(QuestionValuation questionValuation)
        {
            if(questionValuation == null)
                return new QuestionValuation();

            return questionValuation;
        }
    }
}
