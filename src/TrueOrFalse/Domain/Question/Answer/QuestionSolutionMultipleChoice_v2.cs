using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TrueOrFalse.MultipleChoice;

public class QuestionSolutionMultipleChoice_v2 : QuestionSolution
{
    public List<Choice> Choices = new List<Choice>();

    public void FillFromPostData(NameValueCollection postData)
    {
        List<string> choices =
        (
                from key in postData.AllKeys
                where key.StartsWith("choice-")
                select postData.Get(key)
        )
        .ToList();

        List<string> choicesCorrect =
        (
            from key in postData.AllKeys
            where key.StartsWith("choice_correct-")
            select postData.Get(key)
        )
        .ToList();

        for (int i = 0; i < choices.Count; i++)
        {
            Choices.Add(new Choice
            {
                IsCorrect = choicesCorrect[i] == "Richtige Antwort",
                Text = choices[i]
            });
        }
    }

    public override bool IsCorrect(string answer)
    {
        string[] Answers = answer.Split(new string[] {", "}, StringSplitOptions.RemoveEmptyEntries);
        string[] Solutions = this.CorrectAnswer().Split(new string[] {", "}, StringSplitOptions.RemoveEmptyEntries);
        return Enumerable.SequenceEqual(Answers.OrderBy(t => t), Solutions.OrderBy(t => t));
    }

    public override string CorrectAnswer()
    {
        string CorrectAnswer = "";
        foreach (var SingleChoice in this.Choices)
        {
            if (SingleChoice.IsCorrect == true)
            {
                CorrectAnswer += SingleChoice.Text;
                if (SingleChoice != this.Choices[(this.Choices.Count - 1)])
                    CorrectAnswer += ", ";
            }
        }
        return CorrectAnswer;
    }
}