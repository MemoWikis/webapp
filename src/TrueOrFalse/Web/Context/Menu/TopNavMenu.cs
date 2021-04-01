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
    public Crumbtrail BreadCrumbCategories;
    public IList<TopNavMenuItem> BreadCrumb = new List<TopNavMenuItem>();
    public virtual IList<Category> Categories { get; set; }

    public bool IsCategoryBreadCrumb = true;
    public bool QuestionBreadCrumb = false;
    public bool IsWidgetOrKnowledgeCentral; 

    public bool IsWelcomePage;
}