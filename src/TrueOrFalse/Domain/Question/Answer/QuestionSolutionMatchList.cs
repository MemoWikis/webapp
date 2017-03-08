using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TrueOrFalse.Domain.Question.SolutionType.MatchList;

public class QuestionSolutionMatchList : QuestionSolution
{
    public List<Stem> Stems = new List<Stem>();
    public List<Answer> Answers = new List<Answer>();

    public void FillFromPostData(NameValueCollection postData)
    {
        var postTest = postData;
        List<string> stemsText =
        (
                from key in postData.AllKeys
                where key.StartsWith("stem-")
                select postData.Get(key)
        )
        .ToList();

        List<string> stemAnswerName =
        (
            from key in postData.AllKeys
            where key.StartsWith("stemAnswer-")
            select key
        )
        .ToList();

        //for (int i = 0; i < choices.Count; i++)
        //{
        //    Choices.Add(new Choice
        //    {
        //        IsCorrect = choicesCorrect[i] == "Richtige Antwort",
        //        Text = choices[i]
        //    });
        //}
    }

    public override bool IsCorrect(string answer)
    {
        //string[] Answers = answer.Split(new string[] {", "}, StringSplitOptions.RemoveEmptyEntries);
        //string[] Solutions = this.CorrectAnswer().Split(new string[] {", "}, StringSplitOptions.RemoveEmptyEntries);
        //return Enumerable.SequenceEqual(Answers.OrderBy(t => t), Solutions.OrderBy(t => t));
        return false;
    }

    public override string CorrectAnswer()
    {
        //string CorrectAnswer = "";
        //foreach (var SingleChoice in this.Choices)
        //{
        //    if (SingleChoice.IsCorrect == true)
        //    {
        //        CorrectAnswer += SingleChoice.Text;
        //        if (SingleChoice != this.Choices[(this.Choices.Count - 1)])
        //            CorrectAnswer += ", ";
        //    }
        //}
        //return CorrectAnswer;
        return "false";
    }
}