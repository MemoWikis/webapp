﻿using System.Collections.Specialized;
using SolrNet.Utils;
using TrueOrFalse.Web;

public class QuestionSolutionFlashCard : QuestionSolution
{
    public string Text;

    public void FillFromPostData(NameValueCollection postData)
    {
        Text = postData.Get("FlashCardContent");
    }

    public override bool IsCorrect(string answer)
    {   
        return answer == "(Antwort gewusst)";
    }

    public override string CorrectAnswer()
    {
        return HttpUtility.HtmlEncode(Text);
    }

    public override string GetCorrectAnswerAsHtml()
    {
        return MarkdownInit.Run().Transform(Text);
    }
}