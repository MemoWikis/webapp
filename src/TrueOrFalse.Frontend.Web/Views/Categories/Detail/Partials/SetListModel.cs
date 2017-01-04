using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Util;


public class SetListModel : BaseModel
{
    public IList<Set> Sets;

    public Category Category;

    public string BoxTitle;

    public string CallToLearn;

    public bool IncludeLearningButton;

    public SetListModel(Category category, IList<Set> sets, string boxTitle, string callToLearn = null, bool includeButton = false)
    {
        Sets = sets;
        BoxTitle = boxTitle;
        CallToLearn = callToLearn;
        IncludeLearningButton = includeButton;
    }
}
