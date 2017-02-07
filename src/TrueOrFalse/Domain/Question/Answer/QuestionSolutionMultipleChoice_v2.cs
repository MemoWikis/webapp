using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NHibernate.Transform;
using TrueOrFalse.MultipleChoice;

public class QuestionSolutionMultipleChoice_v2
{
    public List<Choice> Choices = new List<Choice>();

    public void FillFromPostData(NameValueCollection postData)
    {
        List<string> Choice =
        (
            from x in postData.AllKeys
            where x.StartsWith("choice-")
            select postData.Get(x)
        )
        .ToList();

        List<string> ChoiceCorrect =
        (
            from x in postData.AllKeys
            where x.StartsWith("choice_correct-")
            select postData.Get(x)
        )
        .ToList();

        for (int i = 0; i < Choice.Count; i++)
        {
            bool ChoiceCorrectBool = ChoiceCorrect[i] == "Richtige Antwort" ? true : false;
            Choices.Add(new Choice {IsCorrect = ChoiceCorrectBool, Text = Choice[i]});
        }
    }
}