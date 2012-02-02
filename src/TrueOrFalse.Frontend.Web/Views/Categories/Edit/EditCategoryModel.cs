using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        var category = new Category(Name) {RelatedCategories = new List<Category>()};
        foreach (var name in RelatedCategories)
            category.RelatedCategories.Add(ServiceLocator.Resolve<CategoryRepository>().GetByName(name));
        return category;
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
    }

    public IEnumerable<string> RelatedCategories { get; set; } 

    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        RelatedCategories = from key in postData.AllKeys where key.StartsWith("cat") select postData[key];
    }
}
