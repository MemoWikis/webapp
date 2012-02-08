using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class CategoriesModel : ModelBase
{

    public IEnumerable<CategoryRowModel> CategoryRows { get; set; } 

    public CategoriesModel(IEnumerable<Category> categories)
    {
        CategoryRows = from category in categories select new CategoryRowModel(category);
    }
}