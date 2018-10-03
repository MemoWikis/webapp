using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Concurrent;

public class TopNavMenuItem
{
    public string Text;
    public string Url;
    public string ToolTipText;

}

public class TopNavMenu
{
    public List<Category> RootCategoriesList;
    public IList<Category> BreadCrumbCategories => Sl.SessionUiData.TopicMenu.CategoryPath;
    public IList<TopNavMenuItem> BreadCrumb = new List<TopNavMenuItem>();
    public virtual IList<Category> Categories { get; set; }

    public bool IsCategoryBreadCrumb = true;
    public bool IsAnswerQuestionOrSetBreadCrumb = false;

    public bool IsWelcomePage;

    public TopNavMenu()
    {
        RootCategoriesList = Sl.CategoryRepo.GetRootCategoriesList();      
    }
}

