using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class EditSubCategoryItemsModel : ModelBase
{
    public EditSubCategoryItemsModel()
    {
    }

    public EditSubCategoryItemsModel(SubCategory subCategory)
    {
        CategoryName = subCategory.Category.Name;
        SubCategoryName = subCategory.Name;
        Items = from item in subCategory.Items select new SubCategoryItemRowModel(item);
    }

    public string CategoryName { get; set; }

    public string SubCategoryName { get; set; }

    public IEnumerable<SubCategoryItemRowModel> Items { get; set; }

    public string NewItem { get; set; }
}