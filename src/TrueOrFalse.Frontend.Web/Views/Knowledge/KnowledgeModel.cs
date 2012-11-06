using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Frontend.Web.Models;


public class KnowledgeModel : ModelBase
{
    private readonly SessionUser _sessionUser;

    public string UserName { get { return _sessionUser.User.Name; } }

    public int TotalAnswerThisWeek;
    public int TotalAnswerThisMonth;
    public int TotalAnswerPreviousWeek;
    public int TotalAnswerLastMonth;

    public int WishKnowledgeCount;

    public KnowledgeModel(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
}