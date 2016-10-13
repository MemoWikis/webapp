using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class AnswerQuestionModel : BaseModel
{
    public Func<UrlHelper, string> PreviousUrl;
    public Func<UrlHelper, string> NextUrl;

    public Guid QuestionViewGuid;

    public int QuestionId;
    public Question Question;
    public User Creator;
    public string CreatorId { get; private set; }
    public string CreatorName { get; private set; }

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

    public string ImageUrl_500px;
    public string SoundUrl;
    public int TotalViews;

    public int TimesAnsweredUser;
    public int TimesAnsweredUserTrue;
    public int TimesAnsweredUserWrong;

    public bool IsOwner;

    public string ImageUrlAddComment;

    public bool HasImage
    {
        get { return !string.IsNullOrEmpty(ImageUrl_500px); }
    }

    public bool HasSound
    {
        get { return !string.IsNullOrEmpty(SoundUrl); }
    }

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
    public IList<SetMini> SetMinis;
    public int SetCount;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public IList<CommentModel> Comments;

    public bool IsLearningSession => LearningSession != null;
    public LearningSession LearningSession;
    public LearningSessionStep LearningSessionStep;
    public int CurrentLearningStepIdx;
    public int CurrentLearningStepPercentage;
    public bool IsLastLearningStep = false;

    public bool IsTestSession;
    public int TestSessionCurrentStep;
    public int TestSessionNumberOfSteps;

    public AnswerQuestionModel()
    {
    }

    public AnswerQuestionModel(Question question)
    {
        HasNextPage = HasPreviousPage = false;
        SourceIsTabAll = true;
        Populate(question);
    }

    public AnswerQuestionModel(Guid questionViewGuid, LearningSession learningSession)
    {
        QuestionViewGuid = questionViewGuid;

        LearningSession = learningSession;

        CurrentLearningStepIdx = LearningSession.CurrentLearningStepIdx();

        LearningSessionStep = LearningSession.Steps[CurrentLearningStepIdx];
        IsLastLearningStep = CurrentLearningStepIdx + 1 == LearningSession.Steps.Count();

        CurrentLearningStepPercentage = CurrentLearningStepIdx == 0
            ? 0
            : (int)Math.Round(CurrentLearningStepIdx/(float)LearningSession.Steps.Count()*100);
        NextUrl = url => url.Action("Learn", Links.AnswerQuestionController,
            new {
                learningSessionId = learningSession.Id,
                learningSessionName = learningSession.UrlName
            });

        Populate(LearningSessionStep.Question);
    }

    public AnswerQuestionModel(TestSession testSession)
    {
        IsTestSession = true;
        TestSessionCurrentStep = testSession.CurrentStep;
        TestSessionNumberOfSteps = testSession.NumberOfSteps;
        var question = Sl.R<QuestionRepo>().GetById(testSession.QuestionIds.ElementAt(testSession.CurrentStep-1));
        Populate(question);
        _sessionUser.TestSession.CurrentStep++;
    }

    public AnswerQuestionModel(Guid questionViewGuid, Question question, QuestionSearchSpec searchSpec)
    {
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

        if (searchSpec.Filter.IsOneCategoryFilter()){
            SourceCategory = Resolve<CategoryRepository>().GetById(searchSpec.Filter.Categories.First());
            if (SourceCategory != null)
                SourceIsCategory = true;
        }

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

        NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new { setId = set.Id, questionId = question.Id });
        PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new { setId = set.Id, questionId = question.Id });

        SourceIsSet = true;
        Set = set;

        Populate(question);
    }

    private void Populate(Question question)
    {
        if (question.Visibility != QuestionVisibility.All)
            if(question.Creator.Id != _sessionUser.User.Id)
                throw new Exception("Invalid access to questionId" + question.Id);

        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepo>().GetBy(question.Id, UserId));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(UserId, question.Id);

        if(IsLoggedIn)
            ImageUrlAddComment = new UserImageSettings(UserId).GetUrl_128px_square(_sessionUser.User.EmailAddress).Url;

        Question = question;

        Comments = Resolve<CommentRepository>()
            .GetForDisplay(question.Id)
            .Select(c => new CommentModel(c))
            .ToList();

        Creator = question.Creator;
        CreatorId = question.Creator.Id.ToString();
        CreatorName = question.Creator.Name;
        CreationDate = question.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(question.DateCreated);

        IsOwner = _sessionUser.IsLoggedInUserOrAdmin(question.Creator.Id);

        QuestionId = question.Id;
        QuestionText = question.Text;
        QuestionTextMarkdown = MardownInit.Run().Transform(question.TextExtended);
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
    }
}