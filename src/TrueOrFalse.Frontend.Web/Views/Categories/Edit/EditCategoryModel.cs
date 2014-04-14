using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;

public class EditCategoryModel : BaseModel
{
    [DisplayName("Name")]
    public string Name { get; set; }

    [DisplayName("Beschreibung")]
    public string Description { get; set; }

    [DisplayName("Wikipedia URL")]
    public string WikipediaURL { get; set; }

    public UIMessage Message;

    public List<string> RelatedCategories = new List<string>();

    public bool IsEditing { get; set; }

    public string ImageUrl { get; set; }

    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }
    public string ImageGuid { get; set; }
    public string ImageLicenceOwner { get; set; }

    public Category Category;

    public EditCategoryModel(){
    }

    public EditCategoryModel(Category category) 
    {
        Init(category);
    }

    public void Init(Category category)
    {
        Category = category;
        Name = category.Name;
        Description = category.Description;
        WikipediaURL = category.WikipediaURL;
        RelatedCategories = (from cat in category.ParentCategories select cat.Name).ToList();
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_350px_square().Url;        
    }

    public Category ConvertToCategory()
    {
        var category = new Category(Name) {ParentCategories = new List<Category>()};
        category.Description = Description;
        category.WikipediaURL = WikipediaURL;
        foreach (var name in RelatedCategories)
            category.ParentCategories.Add(Resolve<CategoryRepository>().GetByName(name));
        return category;
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
        category.Description = Description;
        category.WikipediaURL = WikipediaURL;
        category.ParentCategories.Clear();
        foreach (var name in RelatedCategories)
            category.ParentCategories.Add(Resolve<CategoryRepository>().GetByName(name));
    }
    
    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        RelatedCategories = (from key in postData.AllKeys where key.StartsWith("cat") select postData[key]).ToList();
    }
}
