    using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Script.Serialization;
    using SolrNet.Utils;

public class QuestionSolutionSequence : QuestionSolution
{
    public Dictionary<string, string> Rows;

    public void FillFromPostData(NameValueCollection postData)
    {
        Rows = new Dictionary<string, string>();
        foreach (var rowId in from x in postData.AllKeys where x.StartsWith("key-") select Convert.ToInt32(x.Substring(4)))
        {
            var key = postData.Get("key-" + rowId);
            var value = postData.Get("value-" + rowId);

            Rows.Add(key, value);
        }
    }

    public override bool IsCorrect(string answer)
    {
        var values = new JavaScriptSerializer().Deserialize<string[]>(answer.Trim());
        return values.SequenceEqual(Rows.Values);
    }

    public override string CorrectAnswer()
    {
        return HttpUtility.HtmlEncode(string.Join(", ", Rows.Values));
    }
}