using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class QuestionSetModel : BaseModel
{
    public string Name;
    public IList<Question> Questions;

    public QuestionSetModel(QuestionSet questionSet)
    {
        Name = questionSet.Name;
        Questions = questionSet.Questions;
    }
}
