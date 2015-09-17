﻿using System.Collections.Generic;

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

    public ImageFrontendData ImageFrontendData;

    public string WikiUrl;

    public bool IsOwnerOrAdmin;

    public int CountQuestions;
    public int CountWishQuestions;
    public int CountSets;
    public int CountCreators;

    public int CorrectnesProbability;
    public int AnswersTotal;

    public CategoryModel(Category category)
    {   
        WikiUrl = category.WikipediaURL;
        Category = category;

        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        Type = category.Type.GetShortName();
        IsOwnerOrAdmin = _sessionUser.IsLoggedInUserOrAdmin(category.Creator.Id);

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        var questionRepo = Resolve<QuestionRepo>();
        var wishQuestions = questionRepo.GetForCategoryAndInWishCount(category.Id, UserId, 5);

        CountQuestions = category.CountQuestions +
            R<GetQuestionCount>().Run(UserId, category.Id, new[] { QuestionVisibility.Owner, QuestionVisibility.OwnerAndFriends });
        CountSets = category.CountSets;
        CountCreators = category.CountCreators;
        CountWishQuestions = wishQuestions.Total;

        if(category.Type == CategoryType.Standard)
            TopQuestions = questionRepo.GetForCategory(category.Id, 5, UserId);
        else
            TopQuestions = questionRepo.GetForReference(category.Id, 5, UserId);

        TopWishQuestions = wishQuestions.Items;
        TopSets = Resolve<SetRepo>().GetForCategory(category.Id);

        CategoriesParent = category.ParentCategories;
        CategoriesChildren = Resolve<CategoryRepository>().GetChildren(category.Id);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;
    }
}