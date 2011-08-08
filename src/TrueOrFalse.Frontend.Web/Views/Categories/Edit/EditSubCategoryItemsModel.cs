using System.Collections;
using System.Collections.Generic;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class EditSubCategoryItemsModel : ModelBase
{
    public EditSubCategoryItemsModel(SubCategory subCategory)
    {
        CategoryName = subCategory.Category.Name;
        SubCategoryName = subCategory.Name;
        Items = subCategory.Items;
    }

    public string CategoryName { get; set; }

    public string SubCategoryName { get; set; }

    public IEnumerable<SubCategoryItem> Items { get; set; }
}