using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetExaminedAnswerObjects
{
    public static List<ExaminedAnswerObject> Run(List<List<AnswerHistory>> answerHistoryRows)
    {
        var list = new List<ExaminedAnswerObject>();
        answerHistoryRows.ForEach(r => list.Add(new ExaminedAnswerObject(r)));

        return list;
    }
}
