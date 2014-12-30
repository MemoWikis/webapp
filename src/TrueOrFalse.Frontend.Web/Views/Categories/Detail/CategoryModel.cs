using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class CategoryModel : BaseModel
{
    public int Id;
    public string Name;
    public string Description;
    public string Type;

    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;

    public IList<Set> TopSets;
    public IList<Question> TopQuestions;
    public IList<Question> TopWishQuestions;
    public IList<User> TopCreaters;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public Category Category;

    public string ImageUrl;

    public string WikiUrl;

    public bool IsOwnerOrAdmin;

    public int CountQuestions;
    public int CountWishQuestions;
    public int CountSets;
    public int CountCreators;

    public CategoryModel(Category category)
    {   
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_350px_square().Url;
        WikiUrl = category.WikipediaURL;
        Category = category;

        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        Type = category.Type.GetShortName();
        IsOwnerOrAdmin = _sessionUser.IsOwnerOrAdmin(category.Creator.Id);

        var questionRepo = Resolve<QuestionRepository>();
        var wishQuestions = questionRepo.GetForCategoryAndInWishCount(category.Id, UserId, 5);

        CountQuestions = category.CountQuestions;
        CountSets = category.CountSets;
        CountCreators = category.CountCreators;
        CountWishQuestions = wishQuestions.Total;

        TopQuestions = questionRepo.GetForCategory(category.Id, 5, UserId);
        TopWishQuestions = wishQuestions.Items;
        TopSets = Resolve<SetRepository>().GetForCategory(category.Id);

        CategoriesParent = category.ParentCategories;
        CategoriesChildren = Resolve<CategoryRepository>().GetChildren(category.Id);
    }
}