using System.Collections.Generic;
using System.Linq;

public static class QuestionInSetListExts
{
    public static int GetIndex(this IEnumerable<QuestionInSet> questionsInSets, int questionId)
    {
        var result = -1;
        questionsInSets.Select((x, index) =>
        {
            if (x.Question.Id == questionId)
                result = index;

            return -1;
        }).ToList();

        return result;
    }

    public static QuestionInSet GetNextTo(this IEnumerable<QuestionInSet> questionsInSets, int questionId)
    {
        var index = questionsInSets.GetIndex(questionId);
        if (index < questionsInSets.Count())
            return questionsInSets.ToList()[index + 1];

        return null;
    }

    public static QuestionInSet GetPreviousTo(this IEnumerable<QuestionInSet> questionsInSets, int questionId)
    {
        var index = questionsInSets.GetIndex(questionId);
        if (index > 0)
            return questionsInSets.ToList()[index - 1];

        return null;
    }
}