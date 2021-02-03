using System.Collections.Generic;
using TrueOrFalse.Tools;


public class TopNavMenuItem
{
    public string Text;
    public string Url;
    public string ToolTipText;

}

public class TopNavMenu
{
    public List<Category> RootCategoriesList;
    public IList<Category> BreadCrumbCategories = new List<Category>();
    public IList<TopNavMenuItem> BreadCrumb = new List<TopNavMenuItem>();
    public virtual IList<Category> Categories { get; set; }
    public bool IsWidgetOrKnowledgeCentral = false; 

    public bool IsCategoryBreadCrumb = true;
    public bool IsCategoryLearningBreadCrumb = false;
    public bool SetBreadCrumb = false;
    public bool QuestionBreadCrumb = false;

    public bool IsWelcomePage;

    public TopNavMenu()
    {
        RootCategoriesList = new List<Category>();
        RootCategoriesList.Add(EntityCache.GetCategory(HelperTools.RootCategoryId));
    }
}

