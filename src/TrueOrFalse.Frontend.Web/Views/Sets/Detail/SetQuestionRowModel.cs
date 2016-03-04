﻿using TrueOrFalse;

public class SetQuestionRowModel
{
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }
    public virtual int Sort { get; set; }

    public bool IsInWishknowledge;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public SetQuestionRowModel(
        Question question,
        Set set,
        TotalPerUser totalForUser, 
        QuestionValuation questionValuation)
    {
        Question = question;
        Set = set;
        IsInWishknowledge = questionValuation.IsInWishKnowledge();

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation),
            QuestionValuation = questionValuation ?? new QuestionValuation()
        };
    }
}
