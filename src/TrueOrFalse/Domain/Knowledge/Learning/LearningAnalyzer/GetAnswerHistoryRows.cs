using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetAnswerHistoryRows
{
    public static List<List<AnswerHistory>> Run(List<AnswerHistory> listOfAnswerHistories)
    {
        var completeRows = GetCompleteRows(listOfAnswerHistories);
        return GetSubRows(completeRows);
    }

    public static List<List<AnswerHistory>> GetCompleteRows(List<AnswerHistory> listOfAnswerHistories)
    {
        var lists = new List<List<AnswerHistory>>();
        listOfAnswerHistories.GroupBy(ah => new { ah.UserId, ah.QuestionId })
            .ToList()
            .ForEach(g => lists.Add(g.OrderBy(ah => ah.DateCreated).ThenBy(ah => ah.Id).ToList()));
        return lists;
    }

    public static List<List<AnswerHistory>> GetSubRows(List<List<AnswerHistory>> completeAnswerRows)
    {
        var subRows = new List<List<AnswerHistory>>();
        completeAnswerRows.Where(r => r.Count > 1).ToList()
            .ForEach(r =>
            {
                for (var c = 2; c <= r.Count; c++)
                {
                    subRows.Add(r.GetRange(0, c));
                }    
            });

        return subRows;
    }
}


