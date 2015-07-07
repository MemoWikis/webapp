using System.Collections.Generic;
using System.Linq;

public static class AnswerHistoryExt 
{
    public static List<AnswerHistory> ByQuestionId(this IEnumerable<AnswerHistory> answerHistories, int questionId)
    {
        return answerHistories.Where(item => item.QuestionId == questionId).ToList();
    }
}