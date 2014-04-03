using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class CategoryModel : BaseModel
{
    public int Id;
    public string Name;
    public string Description;

    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;

    public IList<Set> TopSets;
    public IList<Question> TopQuestions = new List<Question>();
    public IList<User> TopCreaters;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public Category Category;

    public string ImageUrl;

    public bool IsOwnerOrAdmin;

    public int CountQuestions;
    public int CountSets;
    public int CountCreators;

    public CategoryModel(Category category)
    {
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_350px_square().Url;
        Category = category;

        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        IsOwnerOrAdmin = _sessionUser.IsOwnerOrAdmin(category.Creator.Id);

        CountQuestions = category.CountQuestions;
        CountSets = category.CountSets;
        CountCreators = category.CountCreators;

        TopQuestions = Resolve<QuestionRepository>().GetForCategory(category.Id, 5);
        TopSets = Resolve<SetRepository>().GetForCategory(category.Id);

        CategoriesParent = category.ParentCategories;
        CategoriesChildren = Resolve<CategoryRepository>().GetChildren(category.Id);
    }
}