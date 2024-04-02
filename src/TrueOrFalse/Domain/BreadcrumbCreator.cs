namespace TrueOrFalse.Domain;

public class BreadcrumbCreator(SessionUser _sessionUser, CrumbtrailService _crumbtrailService) : IRegisterAsInstancePerLifetime
{
    public readonly record struct GetBreadcrumbParam(int wikiId, int currentCategoryId);
    public Breadcrumb GetBreadcrumb( GetBreadcrumbParam param)
    {
        var wikiId = param.wikiId;
        int currentCategoryId = param.currentCategoryId;

        var defaultWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1;
        _sessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetCategory(currentCategoryId);
        var currentWiki = _crumbtrailService.GetWiki(category, _sessionUser);
        _sessionUser.SetWikiId(currentWiki);

        var breadcrumb = _crumbtrailService.BuildCrumbtrail(category, currentWiki);

        var breadcrumbItems = new List<BreadcrumbItem>();

        foreach (var item in breadcrumb.Items)
        {
            if (item.Category.Id != breadcrumb.Root.Category.Id)
                breadcrumbItems.Add(new BreadcrumbItem
                {
                    Name = item.Text,
                    Id = item.Category.Id
                });
        }

        var personalWiki = new BreadcrumbItem();
        if (_sessionUser.IsLoggedIn)
        {
            var personalWikiId = _sessionUser.User.StartTopicId;
            personalWiki.Id = personalWikiId;
            personalWiki.Name = EntityCache.GetCategory(personalWikiId).Name;
        }
        else
        {
            personalWiki.Id = RootCategory.RootCategoryId;
            personalWiki.Name = RootCategory.Get.Name;
        }

        return new Breadcrumb
        {
            NewWikiId = currentWiki.Id,
            Items = breadcrumbItems,
            PersonalWiki = personalWiki,
            RootTopic = new BreadcrumbItem
            {
                Name = breadcrumb.Root.Text,
                Id = breadcrumb.Root.Category.Id
            },
            CurrentTopic = new BreadcrumbItem
            {
                Name = breadcrumb.Current.Text,
                Id = breadcrumb.Current.Category.Id
            },
            BreadcrumbHasGlobalWiki = breadcrumb.Items.Any(c => c.Category.Id == RootCategory.RootCategoryId),
            IsInPersonalWiki = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId == breadcrumb.Root.Category.Id : RootCategory.RootCategoryId == breadcrumb.Root.Category.Id
        };
    }

}
public class Breadcrumb
{
    public int NewWikiId { get; set; }
    public BreadcrumbItem PersonalWiki { get; set; }
    public List<BreadcrumbItem> Items { get; set; }
    public BreadcrumbItem RootTopic { get; set; }
    public BreadcrumbItem CurrentTopic { get; set; }
    public bool BreadcrumbHasGlobalWiki { get; set; }
    public bool IsInPersonalWiki { get; set; }
}
public struct BreadcrumbItem
{
    public string Name { get; set; }
    public int Id { get; set; }
}