using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Domain;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
public class QuestionEditModalControllerLogic: IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepo _questionRepo;
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly UserRepo _userRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public QuestionEditModalControllerLogic(QuestionRepo questionRepo,
        SessionUser sessionUser,
        LearningSessionCache learningSessionCache, 
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        CategoryValuationRepo categoryValuationRepo,
        CategoryRepository categoryRepository,
        ImageMetaDataRepo imageMetaDataRepo,
        UserRepo userRepo,
        QuestionValuationRepo questionValuationRepo) 
    {
        _questionRepo = questionRepo;
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _categoryValuationRepo = categoryValuationRepo;
        _categoryRepository = categoryRepository;
        _imageMetaDataRepo = imageMetaDataRepo;
        _userRepo = userRepo;
        _questionValuationRepo = questionValuationRepo;
    }

    public RequestResult Create(QuestionDataJson questionDataJson)
    {
        if (!LimitCheck.CanSavePrivateQuestion(_sessionUser))
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Subscription.CantSavePrivateQuestion };
        }

        var safeText = RemoveHtmlTags(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Question.MissingText };
        }

        var question = new Question();
        question.Creator = _userRepo.GetById(_sessionUser.UserId);
        question = UpdateQuestion(question, questionDataJson, safeText);

        _questionRepo.Create(question);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionDataJson.IsLearningTab) { }
        _learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem, questionDataJson.SessionIndex, questionDataJson.SessionConfig);

        if (questionDataJson.AddToWishknowledge)
           _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return new RequestResult { success = true, data = LoadQuestion(question.Id) };
    }

    private dynamic LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = SessionUserCache.GetItem(user.Id, _categoryValuationRepo, _userRepo, _questionValuationRepo).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation.ContainsKey(q.Id) && user != null)
        {
            question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return question;
    }

    public RequestResult Edit(QuestionDataJson questionDataJson)
    {
        var safeText = RemoveHtmlTags(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Question.MissingText };
        }

        var question = _questionRepo.GetById(questionDataJson.QuestionId);
        var updatedQuestion = UpdateQuestion(question, questionDataJson, safeText);

        _questionRepo.Update(updatedQuestion);

        if (questionDataJson.IsLearningTab)
            _learningSessionCache.EditQuestionInLearningSession(EntityCache.GetQuestion(updatedQuestion.Id));

        return new RequestResult { success = true, data = LoadQuestion(updatedQuestion.Id) };
    }

    public dynamic GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var solution = question.SolutionType == SolutionType.FlashCard ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml() : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(_permissionCheck.CanView).Distinct();

        return new
        {
            Id = id,
            SolutionType = (int)question.SolutionType,
            Solution = solution,
            SolutionMetadataJson = question.SolutionMetadataJson,
            Text = question.TextHtml,
            TextExtended = question.TextExtendedHtml,
            PublicTopicIds = topicsVisibleToCurrentUser.Select(t => t.Id).ToArray(),
            DescriptionHtml = question.DescriptionHtml,
            Topics = topicsVisibleToCurrentUser.Select(t => FillMiniTopicItem(t)).ToArray(),
            TopicIds = topicsVisibleToCurrentUser.Select(t => t.Id).ToArray(),
            LicenseId = question.LicenseId,
            Visibility = question.Visibility,
        };
    }
    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            Url = Links.CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new CategoryImageSettings(topic.Id).GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(_imageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    private string RemoveHtmlTags(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }

    public class QuestionDataJson
    {
        public int[] CategoryIds { get; set; }
        public int QuestionId { get; set; }
        public string TextHtml { get; set; }
        public string DescriptionHtml { get; set; }
        public dynamic Solution { get; set; }
        public string SolutionMetadataJson { get; set; }
        public int Visibility { get; set; }
        public string SolutionType { get; set; }
        public bool AddToWishknowledge { get; set; }
        public int SessionIndex { get; set; }
        public int LicenseId { get; set; }
        public string ReferencesJson { get; set; }
        public bool IsLearningTab { get; set; }
        public LearningSessionConfig SessionConfig { get; set; }
    }

    private Question UpdateQuestion(Question question, QuestionDataJson questionDataJson, string safeText)
    {
        question.TextHtml = questionDataJson.TextHtml;
        question.Text = safeText;
        question.DescriptionHtml = questionDataJson.DescriptionHtml;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), questionDataJson.SolutionType);

        var preEditedCategoryIds = question.Categories.Select(c => c.Id);
        var newCategoryIds = questionDataJson.CategoryIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var categoryId in categoriesToRemove)
            if (!_permissionCheck.CanViewCategory(categoryId))
                newCategoryIds.Add(categoryId);

        question.Categories = GetAllParentsForQuestion(newCategoryIds, question);
        question.Visibility = (QuestionVisibility)questionDataJson.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var serializer = new JavaScriptSerializer();

            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = questionDataJson.Solution;
            question.Solution = serializer.Serialize(solutionModelFlashCard);
        }
        else
            question.Solution = questionDataJson.Solution;

        question.SolutionMetadataJson = questionDataJson.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(questionDataJson.ReferencesJson))
        {
            var references = ReferenceJson.LoadFromJson(questionDataJson.ReferencesJson, question, _categoryRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = _sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataJson.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem);

        return question;
    }

    private List<Category> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var topics = new List<Category>();
        var privateTopics = question.Categories.Where(c => !_permissionCheck.CanEdit(c)).ToList();
        topics.AddRange(privateTopics);

        foreach (var categoryId in newCategoryIds)
            topics.Add(_categoryRepository.GetById(categoryId));

        return topics;
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;
}