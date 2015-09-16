using System;

public class AnswerFeatureFilter
{
    public static Func<AnswerFeatureFilterParams, bool> Time(int startHour, int endHour)
    {
        return param =>
        {
            if (param.AnswerHistory.DateCreated.Hour >= startHour &&
                param.AnswerHistory.DateCreated.Hour <= endHour)
                return true;

            return false;
        };
    }
    public static Func<AnswerFeatureFilterParams, bool> Repetitions(int times)
    {
        return param =>
        {
            if (param.PreviousAnswers.Count == times)
                return true;

            return false;
        };
    }
}