using System.Collections.Generic;
using System.Linq;

public class AnswerPairFromHistoryRows
{
    /// <summary>
    /// Returns a list of pairs, one foreach user and question.
    /// </summary>
    /// <param name="listOfAnswerHistories"></param>
    public static IList<AnswerPair> Get(IList<AnswerHistory> listOfAnswerHistories)
    {
        var completeRows = GetGroupedByUserAndQuestionId(listOfAnswerHistories);
        return GetPairs(completeRows);
    }

    private static IList<List<AnswerHistory>> GetGroupedByUserAndQuestionId(IList<AnswerHistory> listOfAnswerHistories)
    {
        var lists = new List<List<AnswerHistory>>();
        listOfAnswerHistories
            .GroupBy(ah => new { ah.UserId, ah.QuestionId })
            .ToList()
            .ForEach(grouping => 
                lists.Add(grouping
                    .OrderBy(ah => ah.DateCreated)
                    .ThenBy(ah => ah.Id).ToList()
                )
            );

        return lists;
    }

    private static IList<AnswerPair> GetPairs(IList<List<AnswerHistory>> completeAnswerRows)
    {
        var answerPairs = new List<AnswerPair>();
        completeAnswerRows.Where(r => r.Count > 1).ToList()
            .ForEach(r =>
            {
                for (var c = 2; c <= r.Count; c++)
                    answerPairs.Add(new AnswerPair(r.GetRange(0, c))); 
            });

        return answerPairs;
    }
}