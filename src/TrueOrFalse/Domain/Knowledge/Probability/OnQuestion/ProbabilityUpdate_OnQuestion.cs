public class ProbabilityUpdate_OnQuestion : IRegisterAsInstancePerLifetime
{
    public void Run()
    {
        foreach (var question in Sl.R<QuestionRepository>().GetAll())
            Run(question);
    }

    public void Run(Question question)
    {
        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetBy(question.Id);

        question.CorrectnessProbability =
            Sl.R<ProbabilityCalc_OnQuestion>().Run(answerHistoryItems);

        Sl.R<QuestionRepository>().Update(question);
    }
}