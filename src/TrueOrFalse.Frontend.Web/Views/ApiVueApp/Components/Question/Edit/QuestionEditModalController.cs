﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class QuestionEditModalController(
    SessionUser _sessionUser,
    LearningSessionCache _learningSessionCache,
    PermissionCheck _permissionCheck,
    LearningSessionCreator _learningSessionCreator,
    QuestionInKnowledge _questionInKnowledge,
    CategoryRepository _categoryRepository,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    UserReadingRepo _userReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    QuestionReadingRepo _questionReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    IActionContextAccessor _actionContextAccessor,
    Logg _logg,
    ImageStore _imageStore) : Controller
{
    public readonly record struct QuestionDataRequest(
        int[] CategoryIds,
        int? QuestionId,
        string TextHtml,
        string QuestionExtensionHtml,
        string DescriptionHtml,
        string Solution,
        string SolutionMetadataJson,
        int Visibility,
        SolutionType SolutionType,
        bool? AddToWishknowledge,
        int SessionIndex,
        int LicenseId,
        string ReferencesJson,
        bool IsLearningTab,
        LearningSessionConfig SessionConfig,
        string[] UploadedImagesMarkedForDeletion,
        string[] UploadedImagesInContent
    );

    public readonly record struct CreateResult(
        bool Success,
        string MessageKey,
        QuestionListJson.Question Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateResult Create([FromBody] QuestionDataRequest request)
    {
        if (!new LimitCheck(_logg, _sessionUser).CanSavePrivateQuestion(logExceedance: true))
        {
            return new CreateResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Subscription.CantSavePrivateQuestion
            };
        }

        var safeText = RemoveHtmlTags(request.TextHtml);
        if (safeText.Length <= 0)
        {
            return new CreateResult
            { Success = false, MessageKey = FrontendMessageKeys.Error.Question.MissingText };
        }

        var question = new Question();
        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId);

        question = UpdateQuestion(question, request, safeText);

        _questionWritingRepo.Create(question, _categoryRepository);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (request.UploadedImagesInContent.Length > 0)
            SaveImageToFile.ReplaceTempQuestionContentImages(request.UploadedImagesInContent, question, _questionWritingRepo);

        var deleteImage = new DeleteImage();
        deleteImage.RunForQuestionContentImages(request.UploadedImagesMarkedForDeletion);

        _learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem,
            request.SessionIndex,
            request.SessionConfig);

        if (request.AddToWishknowledge != null && (bool)request.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return new CreateResult { Success = true, Data = LoadQuestion(question.Id) };
    }

    public readonly record struct QuestionEditResult(
        bool Success,
        string MessageKey,
        QuestionListJson.Question Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public QuestionEditResult Edit([FromBody] QuestionDataRequest request)
    {
        var safeText = RemoveHtmlTags(request.TextHtml);
        if (safeText.Length <= 0)
        {
            return new QuestionEditResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingText
            };
        }

        if (request.QuestionId == null)
            return new QuestionEditResult
            { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        var question = _questionReadingRepo.GetById((int)request.QuestionId);
        var updatedQuestion = UpdateQuestion(question, request, safeText);

        _questionWritingRepo.UpdateOrMerge(updatedQuestion, false);

        if (request.IsLearningTab)
            _learningSessionCache.EditQuestionInLearningSession(
                EntityCache.GetQuestion(updatedQuestion.Id));

        var deleteImage = new DeleteImage();
        deleteImage.RunForQuestionContentImages(request.UploadedImagesMarkedForDeletion);

        return new QuestionEditResult { Success = true, Data = LoadQuestion(updatedQuestion.Id) };
    }

    public record struct GetDataResult(
        int Id,
        int SolutionType,
        string Solution,
        string SolutionMetadataJson,
        string Text,
        string TextExtended,
        int[] PublicTopicIds,
        string DescriptionHtml,
        SearchTopicItem[] Topics,
        int[] TopicIds,
        int LicenseId,
        QuestionVisibility Visibility);

    [HttpGet]
    public GetDataResult GetData([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var solution = question.SolutionType == SolutionType.FlashCard
            ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml()
            : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(_permissionCheck.CanView).Distinct();

        return new GetDataResult(
            Id: id,
            SolutionType: (int)question.SolutionType,
            Solution: solution,
            SolutionMetadataJson: question.SolutionMetadataJson,
            Text: question.TextHtml,
            TextExtended: question.TextExtendedHtml,
            PublicTopicIds: topicsVisibleToCurrentUser.Select(t => t.Id).ToArray(),
            DescriptionHtml: question.DescriptionHtml,
            Topics: topicsVisibleToCurrentUser.Select(t => FillMiniTopicItem(t)).ToArray(),
            TopicIds: topicsVisibleToCurrentUser.Select(t => t.Id).ToArray(),
            LicenseId: question.LicenseId,
            Visibility: question.Visibility
        );
    }

    [HttpGet]
    public int GetCurrentQuestionCount([FromRoute] int id) => EntityCache.GetCategory(id)
        .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;

    private QuestionListJson.Question LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = _extendedUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = new Links(_actionContextAccessor, _httpContextAccessor).GetUrl(q);
        question.ImageData = new ImageFrontendData(
                _imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                _httpContextAccessor,
                _questionReadingRepo)
            .GetImageUrl(40, true)
            .Url;

        var links = new Links(_actionContextAccessor, _httpContextAccessor);
        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
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
            question.HasPersonalAnswer =
                userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return question;
    }

    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new CategoryImageSettings(topic.Id, _httpContextAccessor)
                .GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category)
                .Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    private string RemoveHtmlTags(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }

    private Question UpdateQuestion(Question question, QuestionDataRequest request, string safeText)
    {
        question.TextHtml = request.TextHtml;
        question.Text = safeText;
        question.TextExtendedHtml = request.QuestionExtensionHtml;
        question.DescriptionHtml = request.DescriptionHtml;
        question.SolutionType = request.SolutionType;

        var preEditedCategoryIds = question.Categories.Select(c => c.Id);
        var newCategoryIds = request.CategoryIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var categoryId in categoriesToRemove)
            if (!_permissionCheck.CanViewCategory(categoryId))
                newCategoryIds.Add(categoryId);

        question.Categories = GetAllParentsForQuestion(newCategoryIds, question);
        question.Visibility = (QuestionVisibility)request.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = request.Solution;
            question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);
        }
        else
            question.Solution = request.Solution;

        question.SolutionMetadataJson = request.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(request.ReferencesJson))
        {
            var references =
                ReferenceJson.LoadFromJson(request.ReferencesJson, question, _categoryRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = _sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(request.LicenseId)
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

}