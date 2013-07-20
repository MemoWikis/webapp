using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class CategoriesModel : BaseModel
{
    public Message Message;
    public IEnumerable<CategoryRowModel> CategoryRows { get; set; }

    public int TotalCategories;
    public int TotalMine;
    public string SearchTerm;
    
    public string OrderByLabel;
    public int TotalCategoriesInResult;

    public PagerModel Pager { get; set; }

    public CategoriesModel(IEnumerable<Category> categories, SessionUiData _sessionUi)
    {
        var index = 0;
        CategoryRows = from category in categories select new CategoryRowModel(category, index++);

        Pager = new PagerModel(_sessionUi.SearchSpecCategory);

        TotalCategories = Resolve<GetTotalCategories>().Run(); ;
        TotalMine = 0;

        TotalCategoriesInResult = _sessionUi.SearchSpecCategory.TotalItems;
        OrderByLabel = "..";
    }
}