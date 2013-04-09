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

    public IList<Category> RelatedCategoriesOut;
    public IList<Category> RelatedCategoriesIn;

    public IList<QuestionSet> TopSets;
    public IList<Question> TopQuestions;
    public IList<User> TopCreaters;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public string ImageUrl;

    public bool IsOwner;

    public CategoryModel(Category category)
    {
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_200px_square().Url;

        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        IsOwner = _sessionUser.IsOwner(category.Creator.Id);
    }

}
