using System.Collections.Generic;

public class AnswerPairFromHistoryRows
{
    public static List<AnswerPair> Run(List<List<AnswerHistory>> answerHistoryRows)
    {
        var list = new List<AnswerPair>();
        answerHistoryRows.ForEach(r => list.Add(new AnswerPair(r)));

        return list;
    }
}
