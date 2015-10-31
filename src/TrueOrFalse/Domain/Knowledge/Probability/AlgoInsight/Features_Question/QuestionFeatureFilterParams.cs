using System.Collections.Generic;
using System.Linq;

public class QuestionFeatureFilterParams
{
    public Question Question;
    /// <summary>All answers for this question</summary>
    public IList<AnswerHistory> Answers;

    public IList<AnswerHistory> AllFirstAnswers;

    public IList<AnswerHistory> AllSecondsAnswers;
    public IList<AnswerHistory> AllSecondsOrLaterAnswers;

    public IList<AnswerHistory> AllThirdAnswers;
    public IList<AnswerHistory> AllThirdOrLaterAnswers;

    public QuestionFeatureFilterParams(Question question, IList<AnswerHistory> answers)
    {
        Question = question;
        Answers = answers;

        var historiesPerUser = Answers
            .GroupBy(a => a.UserId)
            .Select(g => g.ToList())
            .ToList();

        AllFirstAnswers = historiesPerUser.Select(h => h.First()).ToList();
        AllSecondsAnswers = historiesPerUser.SelectMany(h => h.Skip(1).Take(1)).ToList();
        AllSecondsOrLaterAnswers = historiesPerUser.SelectMany(h => h.Skip(1)).ToList();

        AllThirdAnswers = historiesPerUser.SelectMany(h => h.Skip(2).Take(1)).ToList();
        AllThirdOrLaterAnswers = historiesPerUser.SelectMany(h => h.Skip(2)).ToList();
    }
}