using System;
using System.Collections.Generic;

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
}