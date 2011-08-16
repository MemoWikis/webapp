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
        Id = subCategory.Id;
        CategoryName = subCategory.Category.Name;
        SubCategoryName = subCategory.Name;
        Items = from item in subCategory.Items select new SubCategoryItemRowModel(item);        
    }

    public int Id { get; set; }

    public string CategoryName { get; set; }

    public string SubCategoryName { get; set; }

    public IEnumerable<SubCategoryItemRowModel> Items { get; set; }

    public string NewItem { get; set; }
}