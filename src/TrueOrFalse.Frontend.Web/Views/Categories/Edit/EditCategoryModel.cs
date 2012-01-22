using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;
using Message = TrueOrFalse.Core.Web.Message;

public class EditCategoryModel : ModelBase
{
    [DisplayName("Name")]
    public string Name { get; set; }

    public Message Message;

    public bool IsEditing { get; set; }

    public EditCategoryModel()
    {
        ShowLeftMenu_Nav();
    }

    public EditCategoryModel(Category category)
    {
        Name = category.Name;
    }

    public Category ConvertToCategory()
    {
        return new Category(Name);
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
    }
}
