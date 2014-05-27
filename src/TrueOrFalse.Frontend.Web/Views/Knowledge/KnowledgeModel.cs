using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Frontend.Web.Models;


public class KnowledgeModel : BaseModel
{
    private new readonly SessionUser _sessionUser;

    public bool IsLoggedIn { get { return _sessionUser.IsLoggedIn;  } }

    public string UserName
    {
        get
        {
            if (_sessionUser.User == null)
                return "Unbekannte(r)";
            return _sessionUser.User.Name;
        }
    }

    public int TotalAnswerThisWeek;
    public int TotalAnswerThisMonth;
    public int TotalAnswerPreviousWeek;
    public int TotalAnswerLastMonth;

    public int QuestionsCount;
    public int QuestionsSetCount;

    public KnowledgeModel(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
}