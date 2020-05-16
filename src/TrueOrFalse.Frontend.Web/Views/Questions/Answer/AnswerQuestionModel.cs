﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;
using static System.String;

public class AnswerQuestionModel : BaseModel
{
    public Func<UrlHelper, string> PreviousUrl;
    public Func<UrlHelper, string> NextUrl;
    public CategoryModel CategoryModel; 

    public Guid QuestionViewGuid;

    public string DescriptionForSearchEngines;
    public string DescriptionForFacebook;

    public int QuestionId;
    public Question Question;
    public UserTinyModel Creator;
    public string CreatorId { get; private set; }
    public string CreatorName { get; private set; }

    public string ImageUrl_250;

    public string PageCurrent;
    public string PagesTotal;
    public string PagerKey;
    public SearchTabType SearchTabOverview;

    public string TotalQualityAvg;
    public string TotalQualityEntries;
    public string TotalRelevanceForAllAvg;
    public string TotalRelevanceForAllEntries;
    public string TotalRelevancePersonalAvg;
    public string TotalRelevancePersonalEntries;
    
    public string SolutionType;
    public QuestionSolution SolutionModel;
    public SolutionMetadata SolutionMetadata;
    public string SolutionMetaDataJson;

    public bool? IsMobileDevice;

    public string ImageUrl_500px;
    public string SoundUrl;
    public int TotalViews;

    public IList<Category> AllCategoriesParents;
    public IList<Category> AllCategorysWithChildrenAndParents { get; set; }
    public IList<Category> ChildrenAndParents; 

    public bool IsOwner;

    public string ImageUrlAddComment;

    public bool HasImage => !IsNullOrEmpty(ImageUrl_500px);
    public bool HasSound => !IsNullOrEmpty(SoundUrl);

    public string CreationDateNiceText { get; private set; }
    public string CreationDate { get; private set; }

    public string AverageAnswerTime { get; private set; }

    public string QuestionText { get; private set; }
    public string QuestionTextMarkdown { get; private set; }

    public QuestionVisibility Visibility { get; private set; }

    public bool HasPreviousPage;
    public bool HasNextPage;

    public bool SourceIsTabAll;
    public bool SourceIsTabMine;
    public bool SourceIsTabWish;

    public bool SourceIsSet;
    public bool SourceIsCategory;
    public Category SourceCategory;

    public Set Set;
    public IList<Category> Categories;
    public Category PrimaryCategory;
    public IList<SetMini> SetMinis;
    public int SetCount;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public IList<CommentModel> Comments;
    public int CommentsSettledCount = 0;

    public bool IsLearningSession => LearningSession != null;
    public LearningSessionNew  LearningSession;
    public LearningSessionStepNew LearningSessionStep;
    public int CurrentLearningStepIdx;
    public int CurrentLearningStepPercentage;
    public bool IsLastLearningStep = false;

    public TestSession TestSession;
    public bool IsTestSession;
    public int TestSessionId;
    public int TestSessionCurrentStep;
    public int TestSessionNumberOfSteps;
    public int TestSessionCurrentStepPercentage;
    public bool TestSessionIsLastStep = false;
    public int TestSessionProgessAfterAnswering;

    public bool DisableCommentLink;
    public bool DisableAddKnowledgeButton;
    public bool IsInWidget;
    public bool ShowCategoryList = true;

    public ContentRecommendationResult ContentRecommendationResult;
    public AnalyticsFooterModel AnalyticsFooterModel;
    public bool QuestionHasParentCategories = false;

    public AnswerQuestionModel(Question question, bool? isMobileDevice = null, bool showCategoryList = true, CategoryModel categoryModel= null)
    {
        if(this.QuestionViewGuid == Guid.Empty)
            QuestionViewGuid = Guid.NewGuid();
        CategoryModel = categoryModel;
        IsMobileDevice = isMobileDevice;
        HasNextPage = HasPreviousPage = false;
        SourceIsTabAll = true;
        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        ShowCategoryList = showCategoryList;

        Populate(question);
    }

    public AnswerQuestionModel(LearningSessionNew learningSession, bool? isMobileDevice = null)
    {
        this.IsMobileDevice = isMobileDevice;

        LearningSession = learningSession;

        CurrentLearningStepIdx = LearningSession.CurrentIndex;

        LearningSessionStep = LearningSession.Steps[CurrentLearningStepIdx];
//LearningSessionStep.Question = Sl.QuestionRepo.GetById(LearningSessionStep.Question.Id);//Prevents nhibernate lazy load exception

        IsLastLearningStep = CurrentLearningStepIdx + 1 == LearningSession.Steps.Count();

        CurrentLearningStepPercentage = CurrentLearningStepIdx == 0
            ? 0
            : (int)Math.Round(CurrentLearningStepIdx/(float)LearningSession.Steps.Count()*100);

        //NextUrl = url => url.Action("Learn", Links.AnswerQuestionController,
        //    new
        //    {
        //        learningSessionId = learningSession.Id,
        //        learningSessionName = learningSession.UrlName
        //    });

        Populate(LearningSessionStep.Question);
    }

    //public AnswerQuestionModel(int dummyQuestionId, bool testSession = false)
    //{
    //    var dummyQuestion = Sl.QuestionRepo.GetById(dummyQuestionId);

    //    //LearningSession = new LearningSessionNew{Steps = new List<LearningSessionStep>()};      

    //    for (var i = 0; i < LearningSession.DefaultNumberOfSteps; i++)
    //    {
    //        LearningSession.Steps.Add(new LearningSessionStep { Idx = i, Question = dummyQuestion });
    //    }

    //    Populate(dummyQuestion);

    //}

    public AnswerQuestionModel(TestSession testSession, Guid questionViewGuid, Question question, bool? isMobileDevice = null)
    {
        this.IsMobileDevice = isMobileDevice;

        QuestionViewGuid = questionViewGuid;
        TestSession = testSession;
        IsTestSession = true;
        TestSessionId = testSession.Id;
        TestSessionCurrentStep = testSession.CurrentStepIndex;
        TestSessionNumberOfSteps = testSession.NumberOfSteps;
        TestSessionIsLastStep = testSession.CurrentStepIndex == testSession.NumberOfSteps;
        TestSessionCurrentStepPercentage = TestSessionCurrentStep == 0
            ? 0
            : (int) Math.Round((TestSessionCurrentStep-1)/(float) TestSessionNumberOfSteps*100);
        TestSessionProgessAfterAnswering = (int)Math.Round((TestSessionCurrentStep) / (float)TestSessionNumberOfSteps * 100);
        NextUrl = url => url.Action("Test", Links.AnswerQuestionController);
        HasNextPage = true;
        Populate(question);
    }


    //we have no widgets, this can deleted
    public AnswerQuestionModel(Guid questionViewGuid, Question question, QuestionSearchSpec searchSpec, bool? isMobileDevice = null)
    {
        this.IsMobileDevice = isMobileDevice;
        QuestionViewGuid = questionViewGuid;

        PageCurrent = searchSpec.CurrentPage.ToString();
        PagesTotal = searchSpec.PageCount.ToString();
        PagerKey = searchSpec.Key;
        SearchTabOverview = searchSpec.SearchTab;
        HasPreviousPage = searchSpec.HasPreviousPage();
        HasNextPage = searchSpec.HasNextPage();

        NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new {pager = PagerKey});
        PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new {pager = PagerKey});

        SourceIsTabAll = SearchTabType.All == searchSpec.SearchTab;
        SourceIsTabMine = SearchTabType.Mine == searchSpec.SearchTab;
        SourceIsTabWish = SearchTabType.Wish == searchSpec.SearchTab;

        if (searchSpec.Filter.HasExactOneCategoryFilter()){
            SourceCategory = Resolve<CategoryRepository>().GetById(searchSpec.Filter.Categories.First());
            if (SourceCategory != null)
                SourceIsCategory = true;
        }

        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        Populate(question);
    }

    public AnswerQuestionModel(Guid questionViewGuid, Set set, Question question)
    {
        QuestionViewGuid = questionViewGuid;

        int pageCurrent = set.QuestionsInSet.GetIndex(question.Id) + 1;
        int pagesTotal = set.QuestionsInSet.Count;
        PageCurrent = pageCurrent.ToString();
        PagesTotal = pagesTotal.ToString();

        HasPreviousPage = pageCurrent > 1;
        HasNextPage = pageCurrent < pagesTotal;

        if (HasNextPage)
            NextUrl = url => Links.AnswerQuestion(url, set.QuestionsInSet.GetNextTo(question.Id).Question, set);

        if (HasPreviousPage)
            PreviousUrl = url => Links.AnswerQuestion(url, set.QuestionsInSet.GetPreviousTo(question.Id).Question, set);

        SourceIsSet = true;
        Set = set;

        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        Populate(question);
    }

    private void Populate(Question question)
    {
        Creator = new UserTinyModel(question.Creator);

        if (question.Visibility != QuestionVisibility.All)
            if(Creator.Id != _sessionUser.User.Id || IsTestSession)
                throw new Exception("Invalid access to questionId" + question.Id);

        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, UserId));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(UserId, question.Id);

        if(IsLoggedIn)
            ImageUrlAddComment = new UserImageSettings(UserId).GetUrl_128px_square(_sessionUser.User).Url;

        Question = question;

        var comments = Resolve<CommentRepository>().GetForDisplay(question.Id);
        Comments = comments
            .Where(c => !c.IsSettled)
            .Select(c => new CommentModel(c))
            .ToList();
        CommentsSettledCount = comments.Count(c => c.IsSettled);

       
        CreatorId = Creator.Id.ToString();
        CreatorName = Creator.Name;
        CreationDate = question.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(question.DateCreated);

        IsOwner = _sessionUser.IsLoggedInUserOrAdmin(Creator.Id);

        var imageResult = new UserImageSettings(Creator.Id).GetUrl_250px(Creator);
        ImageUrl_250 = imageResult.Url;

        QuestionId = question.Id;
        QuestionText = question.Text;
        QuestionTextMarkdown = question.TextExtended != null ? MarkdownMarkdig.ToHtml(question.TextExtended) : "";
        Visibility = question.Visibility;
        SolutionType = question.SolutionType.ToString();
        SolutionModel = GetQuestionSolution.Run(question);

        SolutionMetadata = new SolutionMetadata {Json = question.SolutionMetadataJson};
        SolutionMetaDataJson = question.SolutionMetadataJson;

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };

        IsInWishknowledge = questionValuationForUser.IsInWishKnowledge();
        
        TotalViews = question.TotalViews + 1;

        TotalQualityAvg = question.TotalQualityAvg.ToString();
        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalRelevanceForAllAvg = question.TotalRelevanceForAllAvg.ToString();
        TotalRelevanceForAllEntries = question.TotalRelevanceForAllEntries.ToString();
        TotalRelevancePersonalAvg = question.TotalRelevancePersonalAvg.ToString();
        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();

        AverageAnswerTime = "";

        ImageUrl_500px = QuestionImageSettings.Create(question.Id).GetUrl_128px().Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);

        Categories = question.Categories;
        SetMinis = question.SetTop5Minis;
        SetCount = question.SetsAmount;


        QuestionHasParentCategories = question.Categories.Any();
        //Find best suited primary category for question
        if (!IsTestSession && !IsLearningSession && QuestionHasParentCategories)
        {
            PrimaryCategory = GetPrimaryCategory.GetForQuestion(question);
            AnalyticsFooterModel = new AnalyticsFooterModel(PrimaryCategory, true);
            AllCategoriesParents = Sl.CategoryRepo.GetAllParents(PrimaryCategory.Id);
            var allCategoryChildrens = Sl.CategoryRepo.GetChildren(PrimaryCategory.Id);
            AllCategorysWithChildrenAndParents = question.Categories.Concat(allCategoryChildrens).Concat(AllCategoriesParents).ToList();
            ChildrenAndParents = allCategoryChildrens.Concat(AllCategoriesParents).ToList();
        }

        DescriptionForSearchEngines = GetMetaDescriptionSearchEngines();
        DescriptionForFacebook = GetMetaDescriptionsFacebook();

        var authors = Sl.QuestionRepo
            .GetAuthorsQuestion(QuestionId, filterUsersForSidebar:true)
            .ToList();
        SidebarModel.Fill(authors, UserId);
    }

    private string GetMetaDescriptionSearchEngines()
    {
        var result = "";

        if (Question.SolutionType == TrueOrFalse.SolutionType.MultipleChoice_SingleSolution)
        {
            result = $"Antwort: '{SolutionModel.GetAnswerForSEO()}'. {Environment.NewLine}";

            if (result.Length < 100 && !IsNullOrEmpty(Question.Description))
            {
                result += Environment.NewLine;
                result += "Erkärung: ";
                result += Question.Description;
            }

            if (result.Length < 50)
            {
                result += "Alternativen: ";
                result += ((QuestionSolutionMultipleChoice_SingleSolution)SolutionModel)
                    .Choices
                    .Skip(1)
                    .Aggregate((a, b) => a + ", " + b) + "?  ";
            }

        }
        else
        {
            result += Question.Description;
        }

        return result.Truncate(300, addEllipsis: true).Trim().Replace("\"", "'");
    }

    private string GetMetaDescriptionsFacebook()
    {
        var result = "";

        if (Question.SolutionType == TrueOrFalse.SolutionType.MultipleChoice_SingleSolution)
        {
            var solutionModel = (QuestionSolutionMultipleChoice_SingleSolution) SolutionModel;

            if (!solutionModel.Choices.Any())
                return "";

            result = solutionModel
                .Choices
                .Shuffle()
                .Aggregate((a, b) => $"{a} - oder - {Environment.NewLine} {b}");
        }

        return result;
    }
}
