using System.Collections.Generic;
using System.Linq;

public static class AnswerHistoryExt 
{
    public static AnswerHistory ByQuestionId(this IEnumerable<AnswerHistory> answerHistories, int questionId)
    {
        return answerHistories.FirstOrDefault(item => item.QuestionId == questionId);
    }
}