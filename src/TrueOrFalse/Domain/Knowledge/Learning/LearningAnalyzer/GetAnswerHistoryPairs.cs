using System.Collections.Generic;
using System.Linq;

public class GetAnswerHistoryPairs
{
    /// <summary>
    /// Returns a list of pairs, one foreach user and question.
    /// </summary>
    /// <param name="listOfAnswerHistories"></param>
    public static List<AnswerPair> Run(List<AnswerHistory> listOfAnswerHistories)
    {
        var completeRows = GetGroupedByUserAndQuestionId(listOfAnswerHistories);
        return GetPairs(completeRows);
    }

    private static List<List<AnswerHistory>> GetGroupedByUserAndQuestionId(List<AnswerHistory> listOfAnswerHistories)
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

    private static List<AnswerPair> GetPairs(List<List<AnswerHistory>> completeAnswerRows)
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