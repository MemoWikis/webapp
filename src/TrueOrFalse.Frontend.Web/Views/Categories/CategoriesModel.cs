using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;
using static System.String;

public class CategoriesModel : BaseModel
{
    public UIMessage Message;

    public bool ActiveTabAll = true;
    public bool ActiveTabWish;

    public IEnumerable<CategoryRowModel> Rows;

    public string CanonicalUrl;
    public bool HasFiltersOrChangedOrder;
    public string PageTitle = "Themen";
    public int TotalCategoriesInSystem { get; set; }
    public int TotalWish  { get; set; }
    public string SearchTerm  { get; set; }

    public string OrderByLabel { get; set; }
    public CategorytOrderBy OrderBy;

    public int TotalCategoriesInResult;

    public PagerModel Pager;
    public string SearchUrl;

    public string Suggestion;
    public CategoriesSearchResultModel SearchResultModel;

    public CategoriesModel()
    {
    }

    public CategoriesModel(IList<Category> categories, CategorySearchSpec searchSpec, SearchTabType searchTab)
    {
        Init(categories, searchSpec, searchTab);
    }

    public void Init(IList<Category> categories, CategorySearchSpec searchSpec, SearchTabType searchTab)
    {
        SetCategories(categories);

        ActiveTabAll = searchTab == SearchTabType.All;
        ActiveTabWish = searchTab == SearchTabType.Wish;

        Pager = new PagerModel(searchSpec){
            Controller = Links.CategoriesController
        };

        if (ActiveTabAll)
        {
            Pager.Action = Links.CategoriesAction;
            SearchUrl = "/Kategorien/Suche";
        }
        else if (ActiveTabWish)
        {
            Pager.Action = Links.CategoriesWishAction;
            SearchUrl = "/Kategorien/Wunschwissen/Suche";
        }

        Suggestion = searchSpec.GetSuggestion();

        TotalCategoriesInSystem = GetCategoriesCount.All();
        TotalWish = GetCategoriesCount.Wish(UserId);

        SearchTerm = searchSpec.SearchTerm;
        TotalCategoriesInResult = searchSpec.TotalItems;

        OrderByLabel = searchSpec.OrderBy.ToText();
        OrderBy = searchSpec.OrderBy;

        SearchResultModel = new CategoriesSearchResultModel(this);
        if (!IsNullOrEmpty(searchSpec.SearchTerm) ||
            !(searchSpec.OrderBy.BestMatch.IsCurrent() || IsNullOrEmpty(OrderByLabel)))
            HasFiltersOrChangedOrder = true;

        if (ActiveTabAll)
            CanonicalUrl = Links.CategoriesAll();
        else if (ActiveTabWish)
            CanonicalUrl = Links.CategoriesWish();

        if (Pager.CurrentPage > 1)
        {
            CanonicalUrl += "?page=" + Pager.CurrentPage;
            PageTitle += " (Seite " + Pager.CurrentPage + ")";
        }
    }

    public void SetCategories(IList<Category> categories)
    {
        var valuations = R<CategoryValuationRepo>().GetBy(categories.GetIds().ToArray(), _sessionUser.UserId);

        Rows = 
            from category 
            in categories
            select 
                new CategoryRowModel(
                    category, 
                    NotNull.Run(valuations.ByCategoryId(category.Id))
                );
    }
}