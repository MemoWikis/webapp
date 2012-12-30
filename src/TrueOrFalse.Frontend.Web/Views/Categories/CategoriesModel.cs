using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;

public class CategoriesModel : BaseModel
{

    public Message Message;
    public IEnumerable<CategoryRowModel> CategoryRows { get; set; } 

    public CategoriesModel(IEnumerable<Category> categories)
    {
        CategoryRows = from category in categories select new CategoryRowModel(category);
    }
}