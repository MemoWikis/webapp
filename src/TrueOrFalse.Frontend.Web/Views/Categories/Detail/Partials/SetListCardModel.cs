using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;


public class SetListCardModel : BaseContentModule
{
    public IList<Set> Sets;

    public string Title;
    public int TitleRowCount;
    public string Description;
    public int DescriptionRowCount;
    public int SetRowCount;
    public int CategoryId;
    public string TestLink;

    public SetListCardModel(IList<Set> sets, string title, string description, int categoryId, int titleRowCount = -1, int descriptionRowCount = -1, int setRowCount = -1)
    {
        Sets = sets;
        Title = title;
        TitleRowCount = titleRowCount;
        Description = description;
        DescriptionRowCount = descriptionRowCount;
        SetRowCount = setRowCount;
        CategoryId = categoryId;
    }
}
