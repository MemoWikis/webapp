using System.Collections.Generic;
using System.Linq;

public static class AnswerExt 
{
    public static List<Answer> ByQuestionId(this IEnumerable<Answer> answerHistories, int questionId)
    {
        return answerHistories.Where(item => item.Question.Id == questionId).ToList();
    }

    public static List<Answer> By(this IEnumerable<Answer> answerHistories, Question question, User user)
    {
        if(question == null || user == null)
            return new List<Answer>();

        return answerHistories.Where(item => 
            item.Question.Id == question.Id &&
            item.UserId == user.Id
        ).ToList();
    }

    /// <returns>A value between 0 and 1</returns>
    public static double AverageCorrectness(this IEnumerable<Answer> answerHistories)
    {
        return answerHistories.Average(x =>
        {
            switch (x.AnswerredCorrectly)
            {
                case AnswerCorrectness.MarkedAsTrue:
                case AnswerCorrectness.True:
                    return 1;
                case AnswerCorrectness.False:
                    return 0;
                default: 
                    throw new Exception("unknown type");
            }
        });
    }
}