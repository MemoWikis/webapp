using TrueOrFalse;

public class SetQuestionRow
{
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }
    public virtual int Sort { get; set; }

    public HistoryAndProbabilityModel HistoryAndProbability;

    public SetQuestionRow(Question question, TotalPerUser totalForUser, QuestionValuation questionValuation)
    {
        Question = question;
        Set = Set;

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation),
            QuestionValuation = questionValuation ?? new QuestionValuation()
        };
    }
}
