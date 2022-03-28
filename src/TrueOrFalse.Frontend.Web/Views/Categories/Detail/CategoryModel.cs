﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using Seedworks.Web.State;
using TrueOrFalse.Web;

public class CategoryModel : BaseContentModule
{
    public string MetaTitle;
    public string MetaDescription;

    public int Id;
    public string Name;
    public string Description;

    public KnowledgeSummary KnowledgeSummary;

    public string CustomPageHtml;//Is set in controller because controller context is needed
    public CategoryChange CategoryChange;//Is set in controller because controller context is needed
    public bool NextRevExists;   //Is set in controller because controller context is needed
    public IList<CategoryCacheItem> CategoriesParent;
    public IList<CategoryCacheItem> CategoriesChildren;
    public IList<QuestionCacheItem> AggregatedQuestions;
    public IList<QuestionCacheItem> CategoryQuestions;
    public int AggregatedTopicCount;
    public bool IsInTopicTab = false;
    public bool IsInLearningTab = false;
    public bool IsInAnalyticsTab = false;
    public UserTinyModel Creator;
    public string CreatorName;
    public string CreationDate;
    public string ImageUrl_250;
    public CategoryCacheItem Category;
    public ImageFrontendData ImageFrontendData;
    public string WikipediaURL;
    public string Url;
    public bool IsOwnerOrAdmin;
    public bool IsTestSession = false;
    public bool IsLearningSession = true;
    public int CountAggregatedQuestions;
    public int CountCategoryQuestions;
    public int CountReferences;
    public int CountWishQuestions;
    public int CountSets;
    public const int MaxCountQuestionsToDisplay = 20;
    public int CorrectnesProbability;
    public int AnswersTotal;
    private readonly QuestionRepo _questionRepo;
    private readonly CategoryRepository _categoryRepo;
    public bool IsInWishknowledge;
    public bool IsLearningTab;
    public string TotalPins;
    public AnalyticsFooterModel AnalyticsFooterModel;
    public bool CategoryIsDeleted;
    public IList<AuthorViewModel> Authors = new List<AuthorViewModel>();
    public bool ShowLearningSessionConfigurationMessageForTab { get; set; }
    public bool ShowLearningSessionConfigurationMessageForQuestionList { get; set; }
    public bool IsFilteredUserWorld;
    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }

    public string ImageGuid { get; set; }
    public string ImageLicenseOwner { get; set; }
    public bool IsMyWorld { get; set; }
    public bool IsWiki { get; set; }
    public bool HasQuestions = false;


    public EditQuestionModel EditQuestionModel;

    public CategoryModel()
    {
    }

    public CategoryModel(CategoryCacheItem category, bool loadKnowledgeSummary = true, bool isCategoryNull = false)
    {
        if (category == null)
            throw new Exception("category doesn't exist");
        ShowSidebar = true;
        IsMyWorld = UserCache.GetItem(Sl.CurrentUserId).IsFiltered;
        IsWiki = category.IsStartPage();
        var currentRootWiki = CrumbtrailService.GetWiki(category);
        SessionUser.SetWikiId(currentRootWiki);
        TopNavMenu.BreadCrumbCategories = CrumbtrailService.BuildCrumbtrail(category, currentRootWiki);
        CategoryIsDeleted = isCategoryNull;
        AnalyticsFooterModel = new AnalyticsFooterModel(category, false, isCategoryNull);
        MetaTitle = category.Name;
        var safeText = category.Content == null ? null : Regex.Replace(category.Content, "<.*?>", ""); ;

        MetaDescription = SeoUtils.ReplaceDoubleQuotes(safeText).Truncate(250, true);

        _questionRepo = R<QuestionRepo>();
        _categoryRepo = R<CategoryRepository>();

        if (loadKnowledgeSummary)
            KnowledgeSummary = isCategoryNull ? null : KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, UserId);

        var userValuationCategory = UserCache.GetCategoryValuation(UserId, category.Id);
        IsInWishknowledge = userValuationCategory != null && userValuationCategory.IsInWishKnowledge();

        WikipediaURL = category.WikipediaURL;
        Url = category.Url;
        Category = category;
        if (CategoryIsDeleted)
            Category.IsHistoric = true;

        Id = category.Id;
        Name = category.Name;
        Description = string.IsNullOrEmpty(category.Description?.Trim())
                        ? null
                        : MarkdownMarkdig.ToHtml(category.Description);

        Type = category.Type.GetShortName();

        Creator = new UserTinyModel(category.Creator);
        CreatorName = Creator.Name;

        var imageResult = new UserImageSettings(Creator.Id).GetUrl_250px(Creator);
        ImageUrl_250 = imageResult.Url;

        if (category.AuthorIds.Length > 0)
        {
            Authors = AuthorViewModel.Convert(Sl.UserRepo.GetByIds(category.AuthorIds.Distinct().ToList()));
        }
        IsOwnerOrAdmin = Creator != null && SessionUser.IsLoggedInUserOrAdmin(Creator.Id);

        var parentCategories = category.ParentCategories();
        if (parentCategories.All(c => !PermissionCheck.CanView(c)))
        {
            var parents = SearchForParent(parentCategories);
            CategoriesParent = parents;
        }
        else CategoriesParent = parentCategories.Where(PermissionCheck.CanView).ToList();

        CategoriesChildren = UserCache.GetItem(SessionUser.UserId).IsFiltered ?
            UserEntityCache.GetChildren(category.Id, UserId) :
           EntityCache.GetChildren(category.Id, true);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        var wishQuestions = _questionRepo.GetForCategoryAndInWishCount(category.Id, UserId, 5);

        AggregatedQuestions = category.GetAggregatedQuestionsFromMemoryCache();
        CountAggregatedQuestions = AggregatedQuestions.Count;
        CategoryQuestions = category.GetAggregatedQuestionsFromMemoryCache(true, false, category.Id);
        CountCategoryQuestions = CategoryQuestions.Count;

        CountReferences = ReferenceCount.Get(category.Id);


        CountWishQuestions = wishQuestions.Total;

        IsFilteredUserWorld = UserCache.GetItem(SessionUser.UserId).IsFiltered;

        AggregatedTopicCount = IsMyWorld ? CategoriesChildren.Count : GetTotalTopicCount(category);

        TotalPins = category.TotalRelevancePersonalEntries.ToString();

        var editQuestionModel = new EditQuestionModel();
        editQuestionModel.Categories.Add(EntityCache.GetCategory(category.Id));

        EditQuestionModel = editQuestionModel;

        if (CountAggregatedQuestions > 0)
            HasQuestions = true;
    }


    private List<CategoryCacheItem> SearchForParent(IList<CategoryCacheItem> children)
    {
        var parents = new List<CategoryCacheItem>();
        foreach (var child in children)
        {
            parents.AddRange(child.ParentCategories());
        }

        if (parents.Count > 0 && parents.All(c => !PermissionCheck.CanView(c)))
        {
            var parentsToCheck = new List<CategoryCacheItem>(parents);
            parents = SearchForParent(parentsToCheck);
        }

        return parents.DistinctBy(c => c.Id).ToList();
    }

    private QuestionCacheItem GetQuestion(bool hardestQuestion)
    {
        if (CountAggregatedQuestions < 1)
        {
            return null;
        }
        var questions = AggregatedQuestions;
        if (hardestQuestion)
        {
            var question = questions.OrderByDescending(q => q.CorrectnessProbability).Last();
            return question;
        }
        else
        {
            var question = questions.OrderByDescending(q => q.CorrectnessProbability).First();
            return question;
        }
    }

    public ImageUrl GetCategoryImageUrl(CategoryCacheItem category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData).GetImageUrl(232);
    }

    public string GetViews() => Sl.CategoryViewRepo.GetViewCount(Id).ToString();

    public QuestionCacheItem GetDummyQuestion()
    {
        var questionId = 0;
        if (IsMyWorld)
            questionId = Category
               .GetAggregatedQuestionsFromMemoryCache()
               .Where(q => PermissionCheck.CanView(q) && q.IsInWishknowledge())
               .Select(q => q.Id)
               .FirstOrDefault();
        else
            questionId = Category
                   .GetAggregatedQuestionsFromMemoryCache()
                   .Where(q => PermissionCheck.CanView(q))
                   .Select(q => q.Id)
                   .FirstOrDefault();

        return EntityCache.GetQuestionById(questionId);
    }
    public int GetTotalTopicCount(CategoryCacheItem category)
    {
        var user = SessionUser.User;
        return EntityCache.GetChildren(category.Id).Count(PermissionCheck.CanView);
    }

    public bool ShowPinButton()
    {
        if (SessionUser.UserId != -1)
            return !Category.IsHistoric &&
                   !UserCache.GetItem(SessionUser.UserId).User.IsStartTopicTopicId(Category.Id);

        return !Category.IsHistoric;
    }
}