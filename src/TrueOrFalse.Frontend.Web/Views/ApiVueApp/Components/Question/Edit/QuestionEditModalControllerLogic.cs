using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
public class QuestionEditModalControllerLogic : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public QuestionEditModalControllerLogic(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public dynamic Create(QuestionDataJson questionDataJson)
    {
        if (!PermissionCheck.CanSavePrivateQuestion())
        {
            return null;
        }

        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return null;

        var question = new Question();
        question.Creator = Sl.UserRepo.GetById(SessionUser.UserId);
        question = UpdateQuestion(question, questionDataJson, safeText);

        _questionRepo.Create(question);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionDataJson.IsLearningTab) { }
        LearningSessionCache.InsertNewQuestionToLearningSession(questionCacheItem, questionDataJson.SessionIndex, questionDataJson.SessionConfig);

        if (questionDataJson.AddToWishknowledge)
            QuestionInKnowledge.Pin(Convert.ToInt32(question.Id), SessionUser.UserId);

        return LoadQuestion(question.Id);
    }

    private dynamic LoadQuestion(int questionId)
    {
        var user = SessionUser.User;
        var userQuestionValuation = SessionUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = LearningSessionCache.GetLearningSession();
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

    public dynamic Edit(QuestionDataJson questionDataJson)
    {
        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return null;

        var question = Sl.QuestionRepo.GetById(questionDataJson.QuestionId);
        var updatedQuestion = UpdateQuestion(question, questionDataJson, safeText);

        _questionRepo.Update(updatedQuestion);

        if (questionDataJson.IsLearningTab)
            LearningSessionCache.EditQuestionInLearningSession(EntityCache.GetQuestion(updatedQuestion.Id));

        return LoadQuestion(updatedQuestion.Id);
    }

    public dynamic GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var categoryController = new CategoryController();
        var solution = question.SolutionType == SolutionType.FlashCard ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml() : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(PermissionCheck.CanView).Distinct();

        return new
        {
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
            QuestionCount = topic.GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(topic.Id).GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    private string GetSafeText(string text)
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
            if (!PermissionCheck.CanViewCategory(categoryId))
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
            var references = ReferenceJson.LoadFromJson(questionDataJson.ReferencesJson, question);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataJson.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem);

        return question;
    }

    private List<Category> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var categories = new List<Category>();
        var privateCategories = question.Categories.Where(c => !PermissionCheck.CanEdit(c)).ToList();
        categories.AddRange(privateCategories);

        foreach (var categoryId in newCategoryIds)
            categories.Add(Sl.CategoryRepo.GetById(categoryId));

        return categories;
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache().Count;
}