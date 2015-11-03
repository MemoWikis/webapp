using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MaxStreak
{
    public static int GetNumber(List<Answer> listOfAnswers)
    {
        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();
        var count = listOfAnswersOrderedByDate.Count;
        var maxStreak = 0;

        for (var i = 0; i < count; i++)
        {
            var maxStreakInRange = 0;
            var range = listOfAnswersOrderedByDate.GetRange(i, count - i);
            var previousDate = DateTime.MinValue.Date;
            for (var j = 0; j < range.Count; j++)
            {
                if (j == 0)
                {
                    previousDate = range[j].DateCreated.Date;
                    maxStreakInRange++;
                    continue;
                }

                if (range[j].DateCreated.Date == previousDate) continue;

                if (range.All(ah => ah.DateCreated.Date != previousDate.AddDays(1))) break;

                previousDate = range[j].DateCreated.Date;
                maxStreakInRange++;
            }

            if (maxStreakInRange > maxStreak)
            {
                maxStreak = maxStreakInRange;
            }
        }

        return maxStreak;
    }
    public bool IsMatch(List<Answer> listOfAnswers, int days)
    {
        return GetNumber(listOfAnswers) == days;
    }
}

public class MaxStreakOf2 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-2"; } }

    public bool IsMatch(List<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 2);
    }
}

public class MaxStreakOf3 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-3"; } }

    public bool IsMatch(List<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 3);
    }
}

public class MaxStreakOf4 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-4"; } }

    public bool IsMatch(List<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 4);
    }
}

public class MaxStreakOf5 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-5"; } }

    public bool IsMatch(List<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 5);
    }
}

public class MaxStreakOfMoreThan5 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-More-Than-5"; } }

    public bool IsMatch(List<Answer> listOfAnswers)
    {
        return GetNumber(listOfAnswers) > 5;
    }
}

