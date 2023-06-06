using System.Collections.Generic;
using System.Linq;

public class MaxStreak
{
    public static int GetNumber(IList<Answer> listOfAnswers)
    {
        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();
        var countOfAnswers = listOfAnswersOrderedByDate.Count;
        var maxStreak = 0;

        for (var answerIndex = 0; answerIndex < countOfAnswers; answerIndex++)
        {
            var maxStreakInRange = 0;
            var range = listOfAnswersOrderedByDate.GetRange(answerIndex, countOfAnswers - answerIndex);
            var previousDate = DateTime.MinValue.Date;
            for (var rangeIndex = 0; rangeIndex < range.Count; rangeIndex++)
            {
                if (rangeIndex == 0)
                {
                    previousDate = range[rangeIndex].DateCreated.Date;
                    maxStreakInRange++;
                    continue;
                }

                if (range[rangeIndex].DateCreated.Date == previousDate) continue;

                if (range.All(ah => ah.DateCreated.Date != previousDate.AddDays(1))) break;

                previousDate = range[rangeIndex].DateCreated.Date;
                maxStreakInRange++;
            }

            if (maxStreakInRange > maxStreak)
            {
                maxStreak = maxStreakInRange;
            }
        }

        return maxStreak;
    }
    public bool IsMatch(IList<Answer> listOfAnswers, int days)
    {
        return GetNumber(listOfAnswers) == days;
    }
}

public class MaxStreakOf2 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-2"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 2);
    }
}

public class MaxStreakOf3 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-3"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 3);
    }
}

public class MaxStreakOf4 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-4"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 4);
    }
}

public class MaxStreakOf5 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-5"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 5);
    }
}

public class MaxStreakOfMoreThan5 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-More-Than-5"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return GetNumber(listOfAnswers) > 5;
    }
}

