﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using Message = TrueOrFalse.Web.Message;

public class EditCategoryModel : BaseModel
{
    [DisplayName("Name")]
    public string Name { get; set; }

    public Message Message;

    public List<string> RelatedCategories = new List<string>();

    public bool IsEditing { get; set; }

    public string ImageUrl { get; set; }

    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }
    public string ImageGuid { get; set; }
    public string ImageLicenceOwner { get; set; }

    public EditCategoryModel(){
        ImageUrl = "";
    }

    public EditCategoryModel(Category category) : this()
    {
        Name = category.Name;
        RelatedCategories = (from cat in category.RelatedCategories select cat.Name).ToList();
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_200px_square().Url;
    }

    public Category ConvertToCategory()
    {
        var category = new Category(Name) {RelatedCategories = new List<Category>()};
        foreach (var name in RelatedCategories)
            category.RelatedCategories.Add(Resolve<CategoryRepository>().GetByName(name));
        return category;
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
        category.RelatedCategories.Clear();
        foreach (var name in RelatedCategories)
            category.RelatedCategories.Add(Resolve<CategoryRepository>().GetByName(name));
    }
    
    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        RelatedCategories = (from key in postData.AllKeys where key.StartsWith("cat") select postData[key]).ToList();
    }
}
