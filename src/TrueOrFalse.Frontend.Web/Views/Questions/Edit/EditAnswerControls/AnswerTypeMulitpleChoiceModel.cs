using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class AnswerTypeMulitpleChoiceModel
{
    public List<string> Choices;

    public void FillFromPostData(NameValueCollection postData)
    {
        Choices = (from x in postData.AllKeys where x.StartsWith("choice-") select postData.Get(x)).ToList();
    }
}