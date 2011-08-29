using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;

public class QuestionRowModel
{
    public QuestionRowModel(Question question) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = QuestionId;
    }

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }
}