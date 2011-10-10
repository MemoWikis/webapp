using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;

public class QuestionRowModel
{
    public QuestionRowModel(Question question) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;
    }

    public string CreatorName {get; private set;}

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }
}