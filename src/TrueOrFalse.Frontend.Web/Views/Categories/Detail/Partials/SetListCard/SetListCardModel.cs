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

    public SetListCardModel(int categoryId, SetListCardJson setListCardJson)
    {

        Sets = setListCardJson.SetList;
        Title = setListCardJson.Title;
        TitleRowCount = setListCardJson.TitleRowCount - 1;
        Description = setListCardJson.Description;
        DescriptionRowCount = setListCardJson.DescriptionRowCount - 1;
        SetRowCount = setListCardJson.SetRowCount - 1;
        CategoryId = categoryId;
    }
}
