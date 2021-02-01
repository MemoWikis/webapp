using System.Collections.Generic;

public class TopNavMenuItem
{
    public string Text;
    public string Url;
    public string ToolTipText;
}

public class TopNavMenu
{
    public IList<Category> BreadCrumbCategories = new List<Category>();
    public IList<TopNavMenuItem> BreadCrumb = new List<TopNavMenuItem>();
    public virtual IList<Category> Categories { get; set; }

    public bool IsCategoryBreadCrumb = true;
    public bool QuestionBreadCrumb = false;

    public bool IsWelcomePage;
}