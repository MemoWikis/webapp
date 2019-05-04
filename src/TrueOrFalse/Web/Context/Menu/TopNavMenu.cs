using System.Collections.Generic;


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
    public bool IsWidget = false; 

    public bool IsCategoryBreadCrumb = true;
    public bool IsCategoryLearningBreadCrumb = false;
    public bool IsAnswerQuestionOrSetBreadCrumb = false;

    public bool IsWelcomePage;

    public TopNavMenu()
    {
        RootCategoriesList = Sl.CategoryRepo.GetRootCategoriesList();      
    }
}

