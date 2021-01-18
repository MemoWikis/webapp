using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class QuestionFeatureFilterParams
{
    public Question Question;
    /// <summary>All answers for this question</summary>
    public IList<Answer> Answers;

    public IList<Answer> AllFirstAnswers;

    public IList<Answer> AllSecondsAnswers;
    public IList<Answer> AllSecondsOrLaterAnswers;

    public IList<Answer> AllThirdAnswers;
    public IList<Answer> AllThirdOrLaterAnswers;

    public QuestionFeatureFilterParams(Question question, IList<Answer> answers)
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