using System.Collections.Generic;
using System.Linq;

public static class AnswerHistoryExt 
{
    public static List<AnswerHistory> ByQuestionId(this IEnumerable<AnswerHistory> answerHistories, int questionId)
    {
        return answerHistories.Where(item => item.QuestionId == questionId).ToList();
    }

    public static List<AnswerHistory> By(this IEnumerable<AnswerHistory> answerHistories, Question question, User user)
    {
        if(question == null || user == null)
            return new List<AnswerHistory>();

        return answerHistories.Where(item => 
            item.QuestionId == question.Id &&
            item.UserId == user.Id
        ).ToList();
    }
}