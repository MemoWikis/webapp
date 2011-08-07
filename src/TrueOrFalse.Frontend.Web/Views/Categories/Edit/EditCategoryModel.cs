using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class EditCategoryModel : ModelBase
{
    [DisplayName("Name")]
    public string Name { get; set; }

    public IList<SubCategoryRowModel> SubCategories { get; set; }

    public bool IsEditing { get; set; }

    public EditCategoryModel()
    {
        ShowLeftMenu_Nav();
    }

    public EditCategoryModel(Category category)
    {
        Name = category.Name;
        SubCategories = (from subCategory in category.SubCategories
                           select new SubCategoryRowModel(subCategory)).ToList();
    }

    public Category ConvertToCategory()
    {
        return new Category(Name)
                   {
                       SubCategories = (from model in SubCategories select model.ConvertToSubCategory()).ToList()
                   };
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
        foreach (var subCategoryRowModel in SubCategories)
        {
            SubCategory subCategory = null;
            if (subCategoryRowModel.Id > 0)
            {
                subCategory = category.SubCategories.SingleOrDefault(c => c.Id == subCategoryRowModel.Id);
            }

            if (subCategory == null)
            {
                category.SubCategories.Add(subCategoryRowModel.ConvertToSubCategory());
            }
            else
            {
                subCategoryRowModel.UpdateSubCategory(subCategory);
            }

        }
    }
}
