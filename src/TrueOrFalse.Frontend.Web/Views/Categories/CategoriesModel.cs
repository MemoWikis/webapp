using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class CategoriesModel : BaseModel
{
    public UIMessage Message;

    public bool ActiveTabAll = true;//$temp
    public bool ActiveTabFollowed;//$temp

    public IEnumerable<CategoryRowModel> Rows { get; set; }

    public string CanonicalUrl;
    public bool HasFiltersOrChangedOrder;
    public string PageTitle = "Kategorien";
    public int TotalCategoriesInSystem { get; set; }
    public int TotalMine  { get; set; }
    public string SearchTerm  { get; set; }

    public string OrderByLabel { get; set; }
    public CategorytOrderBy OrderBy;

    public int TotalCategoriesInResult { get; set; }

    public PagerModel Pager { get; set; }

    public string Suggestion;
    public CategoriesSearchResultModel SearchResultModel;

    public void Init(IList<Category> categories)
    {
        SetCategories(categories);
        Pager = new PagerModel(_sessionUiData.SearchSpecCategory){
            Controller = Links.CategoriesController,
            Action = Links.CategoriesAction
        };

        Suggestion = _sessionUiData.SearchSpecCategory.GetSuggestion();

        TotalCategoriesInSystem = Resolve<GetTotalCategories>().Run(); ;
        TotalMine = 0;

        SearchTerm = _sessionUiData.SearchSpecCategory.SearchTerm;

        TotalCategoriesInResult = _sessionUiData.SearchSpecCategory.TotalItems;

        OrderByLabel = _sessionUiData.SearchSpecCategory.OrderBy.ToText();
        OrderBy = _sessionUiData.SearchSpecCategory.OrderBy;

        SearchResultModel = new CategoriesSearchResultModel(this);
        if (!String.IsNullOrEmpty(_sessionUiData.SearchSpecCategory.SearchTerm) ||
            !(_sessionUiData.SearchSpecCategory.OrderBy.BestMatch.IsCurrent() || String.IsNullOrEmpty(OrderByLabel)))
            HasFiltersOrChangedOrder = true;
        CanonicalUrl = Links.Categories();
        if (Pager.CurrentPage > 1)
        {
            CanonicalUrl += "?page=" + Pager.CurrentPage.ToString();
            PageTitle += " (Seite " + Pager.CurrentPage.ToString() + ")";
        }
    }

    public void SetCategories(IList<Category> categories)
    {
        var referenceCounts = ReferenceCount.GetList(
            categories
                .Where(c => c.Type != CategoryType.Standard)
                .Select(c => c.Id).ToList()
        );

        var index = 0;
        Rows = 
            from category 
            in categories
            select 
                new CategoryRowModel(
                    category, 
                    referenceCounts.FirstOrDefault(x => x.CategoryId == category.Id)
                );
    }

}