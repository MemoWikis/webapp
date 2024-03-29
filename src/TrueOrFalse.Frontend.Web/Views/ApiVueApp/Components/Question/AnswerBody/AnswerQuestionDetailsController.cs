﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;


public class AnswerQuestionDetailsController: BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly TotalsPersUserLoader _totalsPersUserLoader;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public AnswerQuestionDetailsController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        TotalsPersUserLoader totalsPersUserLoader,
        IHttpContextAccessor httpContextAccessor,
        SessionUserCache sessionUserCache,
        IActionContextAccessor actionContextAccessor,
        QuestionReadingRepo questionReadingRepo) : base(sessionUser)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _totalsPersUserLoader = totalsPersUserLoader;
        _httpContextAccessor = httpContextAccessor;
        _sessionUserCache = sessionUserCache;
        _actionContextAccessor = actionContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }
    [HttpGet]
    public JsonResult Get([FromRoute] int id) => Json(GetData(id));

    public dynamic GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return Json(null);

        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question, 
            _sessionUser.UserId,
            _totalsPersUserLoader,
            _sessionUserCache);

        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        var hasUserValuation = userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;
        var result = new
        {
            knowledgeStatus = hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            personalProbability = correctnessProbability.CPPersonal,
            personalColor = correctnessProbability.CPPColor,
            avgProbability = correctnessProbability.CPAll,
            personalAnswerCount = history.TimesAnsweredUser,
            personalAnsweredCorrectly = history.TimesAnsweredUserTrue,
            personalAnsweredWrongly = history.TimesAnsweredUserWrong,
            overallAnswerCount = history.TimesAnsweredTotal,
            overallAnsweredCorrectly = history.TimesAnsweredCorrect,
            overallAnsweredWrongly = history.TimesAnsweredWrongTotal,
            isInWishknowledge = answerQuestionModel.HistoryAndProbability.QuestionValuation.IsInWishKnowledge,
            topics = question.CategoriesVisibleToCurrentUser(_permissionCheck).Select(t => new
            {
                Id = t.Id,
                Name = t.Name,
                Url = new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(t.Name, t.Id),
                QuestionCount = t.GetCountQuestionsAggregated(_sessionUser.UserId),
                ImageUrl = new CategoryImageSettings(t.Id, _httpContextAccessor).GetUrl_128px(asSquare: true).Url,
                IconHtml = CategoryCachedData.GetIconHtml(t),
                MiniImageUrl = new ImageFrontendData(
                        _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Category),
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetImageUrl(30, true, false, ImageType.Category).Url,

                Visibility = (int)t.Visibility,
                IsSpoiler = IsSpoilerCategory.Yes(t.Name, question)
            }).Distinct().ToArray(),

            visibility = question.Visibility,
            dateNow,
            endTimer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            creator = new
            {
                id = question.CreatorId,
                name = question.Creator.Name
            },
            creationDate = DateTimeUtils.TimeElapsedAsText(question.DateCreated),
            totalViewCount = question.TotalViews,
            wishknowledgeCount = question.TotalRelevancePersonalEntries,
            license = new
            {
                isDefault = question.License.IsDefault(),
                shortText = question.License.DisplayTextShort,
                fullText = question.License.DisplayTextFull
            }
        };
        return result; 
    }
}