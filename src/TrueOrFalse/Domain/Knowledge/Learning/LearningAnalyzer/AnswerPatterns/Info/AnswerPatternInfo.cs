using System.Collections.Generic;

public class AnswerPatternInfo
{
    public string Name;
    public int MatchedAnswersCount;
    public IList<Answer> Matches;

    public IList<Answer> NextAnswers;
}
