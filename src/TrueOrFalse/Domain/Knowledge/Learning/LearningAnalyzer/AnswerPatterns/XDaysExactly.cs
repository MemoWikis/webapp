using System.Collections.Generic;
using System.Linq;

public class XDaysExactly
{
    public bool IsMatch(IList<Answer> listOfAnswers, int days)
    {
        if (listOfAnswers.Count != days)
            return false;

        return GetNumber(listOfAnswers) == days;
    }

    public static int GetNumber(IList<Answer> listOfAnswers)
    {
        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();
        var numberOfDays = 0;

        var previousDate = DateTime.MinValue;
        for (var i = 0; i < listOfAnswersOrderedByDate.Count; i++)
        {
            if (i == 0)
            {
                previousDate = listOfAnswersOrderedByDate[i].DateCreated;
                numberOfDays++;
                continue;
            }

            if (previousDate.AddDays(1).Date != listOfAnswersOrderedByDate[i].DateCreated.Date)
                break;

            previousDate = listOfAnswersOrderedByDate[i].DateCreated;
            numberOfDays++;
        }

        return numberOfDays;
    }
}

public class XDaysExactly2 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-2"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 2);
    }
}

public class XDaysExactly3 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-3"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 3);
    }
}

public class XDaysExactly4 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-4"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 4);
    }
}

public class XDaysExactly5 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-5"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 5);
    }
}

public class XDaysExactly6OrMore : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-6-Or-More"; } }

    public bool IsMatch(IList<Answer> listOfAnswers)
    {
        return GetNumber(listOfAnswers) > 5;
    }
}
