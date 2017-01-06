using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;


public class SetListCardModel : BaseModel
{
    public IList<Set> Sets;

    public string Title;
    public string Description;
    public int RowCount;
    public int CategoryId;
    public string TestLink;

    public SetListCardModel(IList<Set> sets, string title, string description, int categoryId, int rowCount = -1)
    {
        Sets = sets;
        Title = title;
        Description = description;
        RowCount = rowCount;
        CategoryId = categoryId;
    }
}
