using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Models;


public class KnowledgeModel : ModelBase
{
    private readonly SessionUser _sessionUser;

    public KnowledgeModel(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    public string UserName { get { return _sessionUser.User.Name; } }

    public IEnumerable<SelectListItem> KenDevelopmentTypes
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "Fragen"},
                            new SelectListItem {Text = "Kurse"}
                        };
        }
    }

    public IEnumerable<SelectListItem> KenDevelopmentPeriod
    {
        get
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem {Text = "4 Wochen"},
                            new SelectListItem {Text = "6 Monate"}
                        };
        }
    }

}