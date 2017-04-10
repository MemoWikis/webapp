    using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Script.Serialization;

public class QuestionSolutionFlashCard : QuestionSolution
{
    public string FlashCardContent;

    public void FillFromPostData(NameValueCollection postData)
    {
        FlashCardContent = postData.Get("FlashCardContent");
        //Rows = new Dictionary<string, string>();
        //foreach (var rowId in from x in postData.AllKeys where x.StartsWith("key-") select Convert.ToInt32(x.Substring(4)))
        //{
        //    var key = postData.Get("key-" + rowId);
        //    var value = postData.Get("value-" + rowId);

        //    Rows.Add(key, value);
        //}
    }

    public override bool IsCorrect(string answer)
    {
        //var values = new JavaScriptSerializer().Deserialize<string[]>(answer);
        //return values.SequenceEqual(Rows.Values);
        return false;
    }

    public override string CorrectAnswer()
    {
        return FlashCardContent;
    }
}