using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class CategoriesModel : BaseModel
{
    public Message Message;
    public IEnumerable<CategoryRowModel> CategoryRows { get; set; }

    public int TotalCategories { get; set; }
    public int TotalMine  { get; set; }
    public string SearchTerm  { get; set; }
    
    public string OrderByLabel  { get; set; }
    public int TotalCategoriesInResult { get; set; }

    public PagerModel Pager { get; set; }

    public void Init(IEnumerable<Category> categories, SessionUiData _sessionUi)
    {
        SetCategories(categories);
        Pager = new PagerModel(_sessionUi.SearchSpecCategory){
            Controller = Links.CategoriesController,
            Action = Links.Categories
        };

        TotalCategories = Resolve<GetTotalCategories>().Run(); ;
        TotalMine = 0;

        SearchTerm = _sessionUi.SearchSpecCategory.SearchTearm;

        TotalCategoriesInResult = _sessionUi.SearchSpecCategory.TotalItems;
        OrderByLabel = "..";        
    }

    public void SetCategories(IEnumerable<Category> categories)
    {
        var index = 0;
        CategoryRows = from category in categories select new CategoryRowModel(category, index++);
    }

}