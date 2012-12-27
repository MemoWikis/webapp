using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class CategoriesModel : BaseModel
{

    public IEnumerable<CategoryRowModel> CategoryRows { get; set; } 

    public CategoriesModel(IEnumerable<Category> categories)
    {
        CategoryRows = from category in categories select new CategoryRowModel(category);
    }
}