using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class QuestionSetModel : BaseModel
{
    public int Id;
    public string Name;
    public IList<Question> Questions;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public bool IsOwner;

    public QuestionSetModel(QuestionSet questionSet)
    {
        Id = questionSet.Id;
        Name = questionSet.Name;
        Questions = questionSet.Questions;

        IsOwner = _sessionUser.IsOwner(questionSet.Creator.Id);

        Creator = questionSet.Creator;
        CreatorName = questionSet.Creator.Name;
        CreationDate = questionSet.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = TimeElapsedAsText.Run(questionSet.DateCreated);
    }
}
