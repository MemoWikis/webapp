﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using TrueOrFalse.Domain.Question.Answer;

public class AnswerQuestionDetailsController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    TotalsPerUserLoader totalsPerUserLoader,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    QuestionReadingRepo _questionReadingRepo,
    QuestionViewRepository _questionViewRepository) : Controller
{
    [HttpGet]
    public AnswerQuestionDetailsResult? Get([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return null;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Logg.r.Information("AnswerQuestionDetailsTimer a - {0}", stopWatch.ElapsedMilliseconds);
        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser,
            totalsPerUserLoader,
            _extendedUserCache);

        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;
        var sessionUser = _extendedUserCache.GetItem(_sessionUser.UserId);
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? sessionUser.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        Logg.r.Information("AnswerQuestionDetailsTimer b - {0}", stopWatch.ElapsedMilliseconds);
        var hasUserValuation =
            userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;
        stopWatch.Stop();

        var result = new AnswerQuestionDetailsResult(
            KnowledgeStatus: hasUserValuation
                ? userQuestionValuation[question.Id].KnowledgeStatus
                : KnowledgeStatus.NotLearned,
            PersonalProbability: correctnessProbability.CPPersonal,
            PersonalColor: correctnessProbability.CPPColor,
            AvgProbability: correctnessProbability.CPAll,
            PersonalAnswerCount: history.TimesAnsweredUser,
            PersonalAnsweredCorrectly: history.TimesAnsweredUserTrue,
            PersonalAnsweredWrongly: history.TimesAnsweredUserWrong,
            OverallAnswerCount: history.TimesAnsweredTotal,
            OverallAnsweredCorrectly: history.TimesAnsweredCorrect,
            OverallAnsweredWrongly: history.TimesAnsweredWrongTotal,
            IsInWishknowledge: answerQuestionModel.HistoryAndProbability.QuestionValuation
                .IsInWishKnowledge,
            Topics: question.CategoriesVisibleToCurrentUser(_permissionCheck).Select(t =>
                new AnswerQuestionDetailsTopicItem(
                    Id: t.Id,
                    Name: t.Name,
                    QuestionCount: t.GetCountQuestionsAggregated(_sessionUser.UserId),
                    ImageUrl: new PageImageSettings(t.Id, _httpContextAccessor)
                        .GetUrl_128px(asSquare: true).Url,
                    MiniImageUrl: new ImageFrontendData(
                            _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Page),
                            _httpContextAccessor,
                            _questionReadingRepo)
                        .GetImageUrl(30, true, false, ImageType.Page).Url,
                    Visibility: (int)t.Visibility,
                    IsSpoiler: IsSpoilerCategory.Yes(t.Name, question)
                )).Distinct().ToArray(),
            Visibility: question.Visibility,
            DateNow: dateNow,
            EndTimer: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Creator: new MacroCreator(
                Id: question.CreatorId,
                Name: question.Creator.Name
            ),
            CreationDate: DateTimeUtils.TimeElapsedAsText(question.DateCreated),
            TotalViewCount: _questionViewRepository.GetViewCount(question.Id),
            WishknowledgeCount: question.TotalRelevancePersonalEntries,
            License: new License(
                IsDefault: question.License.IsDefault(),
                ShortText: question.License.DisplayTextShort,
                FullText: question.License.DisplayTextFull
            )
        );
        return result;
    }
}