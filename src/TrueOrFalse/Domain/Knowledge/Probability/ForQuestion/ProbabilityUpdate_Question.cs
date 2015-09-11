public class ProbabilityUpdate_Question : IRegisterAsInstancePerLifetime
{
    public static void Run()
    {
        foreach (var question in Sl.R<QuestionRepository>().GetAll())
            Run(question);
    }

    public static void Run(Question question)
    {
        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetByQuestion(question.Id);

        question.CorrectnessProbability =
            ProbabilityCalc_Question.Run(answerHistoryItems);

        Sl.R<QuestionRepository>().Update(question);
    }
}