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
    public IList<Question> TopQuestions = new List<Question>();
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

        CountQuestions = category.CountQuestions + 
            R<GetQuestionCount>().Run(UserId, category.Id, new[] { QuestionVisibility.Owner, QuestionVisibility.OwnerAndFriends });
        CountSets = category.CountSets;
        CountCreators = category.CountCreators;

        TopQuestions = Resolve<QuestionRepository>().GetForCategory(category.Id, 5, UserId);
        TopSets = Resolve<SetRepository>().GetForCategory(category.Id);

        CategoriesParent = category.ParentCategories;
        CategoriesChildren = Resolve<CategoryRepository>().GetChildren(category.Id);
    }
}