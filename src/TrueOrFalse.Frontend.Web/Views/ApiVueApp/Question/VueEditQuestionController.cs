using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using RazorLight;
using TrueOrFalse;

namespace VueApp;

public class VueEditQuestionControllerVueEditQuestionController(
    SessionUser _sessionUser,
    LearningSessionCache _learningSessionCache,
    PermissionCheck _permissionCheck,
    LearningSessionCreator _learningSessionCreator,
    QuestionInKnowledge _questionInKnowledge,
    CategoryRepository _categoryRepository,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    ImageStore _imageStore,
    SessionUiData _sessionUiData,
    UserReadingRepo _userReadingRepo,
    QuestionChangeRepo _questionChangeRepo,
    QuestionWritingRepo _questionWritingRepo,
    QuestionReadingRepo _questionReadingRepo,
    SessionUserCache _sessionUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor)
    : Controller
{
    public readonly record struct VueEditQuestionResult(
        QuestionListJson.Question Data,
        bool Success,
        string MessageKey,
        string ErrorMsg,
        bool Error,
        string Key);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public VueEditQuestionResult VueCreate(QuestionWritingRepo.QuestionDataParam questionDataParam)
    {
        if (questionDataParam.SessionConfig?.CurrentUserId <= 0)
            questionDataParam.SessionConfig.CurrentUserId = _sessionUser.UserId;

        var safeText = GetSafeText(questionDataParam.TextHtml);
        if (safeText.Length <= 0)
            return new VueEditQuestionResult
            {
                ErrorMsg = "Fehlender Fragetext",
            };

        var question = new Question();
        var sessionUserAsUser = _userReadingRepo.GetById(_sessionUser.UserId);
        question.Creator = sessionUserAsUser;
        question = _questionWritingRepo.UpdateQuestion(question, questionDataParam, safeText);

        _questionWritingRepo.Create(question, _categoryRepository);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionDataParam.IsLearningTab)
        {
        }

        _learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem,
            questionDataParam.SessionIndex, questionDataParam.SessionConfig);

        if (questionDataParam.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return new VueEditQuestionResult
        {
            Success = true,
            Data = new QuestionLoader(
                _sessionUser,
                _sessionUserCache,
                _httpContextAccessor,
                _actionContextAccessor,
                _imageMetaDataReadingRepo,
                _questionReadingRepo,
                _learningSessionCache).LoadQuestion(question.Id)
        };
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public VueEditQuestionResult VueEdit(QuestionWritingRepo.QuestionDataParam questionDataParam)
    {
        var safeText = GetSafeText(questionDataParam.TextHtml);
        if (safeText.Length <= 0)
            return new VueEditQuestionResult
            {
                Error = true,
                Key = "missingText",
            };

        var question = _questionReadingRepo.GetById(questionDataParam.QuestionId);
        var updatedQuestion =
            _questionWritingRepo.UpdateQuestion(question, questionDataParam, safeText);

        _questionWritingRepo.UpdateOrMerge(updatedQuestion, false);

        if (questionDataParam.IsLearningTab)
            _learningSessionCache.EditQuestionInLearningSession(
                EntityCache.GetQuestion(updatedQuestion.Id));

        return new VueEditQuestionResult
        {
            Success = true,
            Data = new QuestionLoader(
                _sessionUser,
                _sessionUserCache,
                _httpContextAccessor,
                _actionContextAccessor,
                _imageMetaDataReadingRepo,
                _questionReadingRepo,
                _learningSessionCache).LoadQuestion(updatedQuestion.Id)
        };
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public VueEditQuestionResult CreateFlashcard(CreateFlashcardParam param)
    {
        var safeText = GetSafeText(param.TextHtml);
        if (safeText.Length <= 0)
            return new VueEditQuestionResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingText
            };

        var question = new Question();

        question.TextHtml = param.TextHtml;
        question.Text = safeText;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), "9");

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = param.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return new VueEditQuestionResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingAnswer
            };

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        var sessionUserAsUser = _userReadingRepo.GetById(_sessionUser.UserId);
        question.Creator = sessionUserAsUser;
        question.Categories =
            _questionReadingRepo.GetAllParentsForQuestion(param.CategoryId, question);
        var visibility = (QuestionVisibility)param.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (param.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        _learningSessionCreator.InsertNewQuestionToLearningSession(
            EntityCache.GetQuestion(question.Id), param.LastIndex, param.SessionConfig);
        return new VueEditQuestionResult
        {
            Success = true,
            Data = new QuestionLoader(
                _sessionUser,
                _sessionUserCache,
                _httpContextAccessor,
                _actionContextAccessor,
                _imageMetaDataReadingRepo,
                _questionReadingRepo,
                _learningSessionCache).LoadQuestion(question.Id)
        };
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }

    public readonly record struct CreateFlashcardParam(
        int CategoryId,
        string TextHtml,
        string Answer,
        int Visibility,
        bool AddToWishknowledge,
        int LastIndex,
        LearningSessionConfig SessionConfig
    );

    [HttpGet]
    public int GetCurrentQuestionCount([FromRoute] int topicId) => EntityCache.GetCategory(topicId)
        .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;

    public readonly record struct StoreImageResult(string PreviewUrl, int NewQuestionId);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public StoreImageResult StoreImage(
        string imageSource,
        int questionId,
        string wikiFileName,
        string uploadImageGuid,
        string uploadImageLicenseOwner,
        string markupEditor
    )
    {
        int newQuestionId = -1;
        Question question;
        if (questionId == -1)
        {
            question = new Question();
            question.Text = string.IsNullOrEmpty(Request.Query["Question"])
                ? "Temporäre Frage"
                : Request.Query["Question"];
            question.Solution = "Temporäre Frage";
            var creator = _userReadingRepo.GetById(_sessionUser.UserId);
            question.Creator = creator;
            question.IsWorkInProgress = true;
            _questionWritingRepo.Create(question, _categoryRepository);

            newQuestionId = questionId = question.Id;
        }
        else
        {
            if (!_permissionCheck.CanEdit(_questionReadingRepo.GetById(questionId)))
                throw new SecurityException("Not allowed to edit question");
        }

        if (imageSource == "wikimedia")
        {
            _imageStore.RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, ImageType.Question, _sessionUser.UserId);
        }

        if (imageSource == "upload")
        {
            _imageStore.RunUploaded<QuestionImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(uploadImageGuid), questionId,
                _sessionUser.UserId, uploadImageLicenseOwner);
        }

        question = _questionReadingRepo.GetById(questionId);
        _questionChangeRepo.AddUpdateEntry(question, imageWasChanged: true);

        var imageSettings =
            new QuestionImageSettings(questionId, _httpContextAccessor, _questionReadingRepo);

        return new StoreImageResult
        {
            PreviewUrl = imageSettings.GetUrl_435px().UrlWithoutTime(),
            NewQuestionId = newQuestionId
        };
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public void PublishQuestions(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            if (questionCacheItem.Creator.Id == _sessionUser.UserId)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public void SetQuestionsToPrivate(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries >
                (questionCacheItem.IsInWishknowledge(_sessionUser.UserId, _sessionUserCache)
                    ? 1
                    : 0);
            if ((questionCacheItem.Creator.Id == _sessionUser.UserId &&
                 !otherUsersHaveQuestionInWuwi) || _sessionUser.IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Owner;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Owner;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEditQuestionModal()
    {
        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Directory.GetCurrentDirectory())
            .UseMemoryCachingProvider()
            .Build();

        string viewPath = "~/Views/Questions/Edit/EditComponents/EditQuestionTemplateLoader.cshtml";
        var html = await engine.CompileRenderAsync(viewPath, new { });

        return Content(html, "text/html");
    }
}