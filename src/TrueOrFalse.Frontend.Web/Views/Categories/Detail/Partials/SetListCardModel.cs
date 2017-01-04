using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Util;


public class SetListCardModel : BaseModel
{
    public IList<Set> Sets;

    public string Title;
    public string Description;

    public SetListCardModel(IList<Set> sets, string title, string description)
    {
        Sets = sets;
        Title = title;
        Description = description;
    }
}
