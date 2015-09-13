using System;

public class AnswerPropertyFilter
{
    public static Func<AnswerHistory, Question, User, bool> Time(int startHour, int endHour)
    {
        return (answerHistory, question, user) =>
        {
            if (answerHistory.DateCreated.Hour >= startHour &&
                answerHistory.DateCreated.Hour <= endHour)
                return true;

            return false;
        };
    }
}