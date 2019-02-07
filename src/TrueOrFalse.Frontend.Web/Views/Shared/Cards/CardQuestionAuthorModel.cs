using System;
using System.Net.Mime;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;

public class CardQuestionAuthorModel : BaseModel
{
    public static CardQuestionAuthorModel GetCardQuestionAuthorModel()
    {
        return new CardQuestionAuthorModel();
    }
}
