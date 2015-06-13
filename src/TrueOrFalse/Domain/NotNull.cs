public class NotNull
{
    public static QuestionValuation Run(QuestionValuation questionValuation)
    {
        if(questionValuation == null)
            return new QuestionValuation();

        return questionValuation;
    }

    public static SetValuation Run(SetValuation setValuation)
    {
        if (setValuation == null)
            return new SetValuation();

        return setValuation;
    }
}