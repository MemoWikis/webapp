using TrueOrFalse;

public class SetQuestionRowModel
{
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }
    public virtual int Sort { get; set; }

    public string CorrectAnswer;

    public bool IsInWishknowledge;
    public bool UserIsInstallationAdmin;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public SetQuestionRowModel(
        Question question,
        Set set,
        TotalPerUser totalForUser, 
        QuestionValuationCacheItem questionValuation)
    {
        Question = question;
        Set = set;
        CorrectAnswer = Question.GetSolution().GetCorrectAnswerAsHtml();

        questionValuation = questionValuation ?? new QuestionValuationCacheItem();

        IsInWishknowledge = questionValuation.IsInWishKnowledge;
        UserIsInstallationAdmin = Sl.SessionUser.IsInstallationAdmin;

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation),
            QuestionValuation = questionValuation
        };
    }
}
