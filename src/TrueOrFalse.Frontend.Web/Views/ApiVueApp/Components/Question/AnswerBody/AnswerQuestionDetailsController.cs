using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System;
using System.Linq;
using TrueOrFalse.Domain.Question.Answer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

public class AnswerQuestionDetailsController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    TotalsPersUserLoader _totalsPersUserLoader,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache extendedUserCache,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    [HttpGet]
    public AnswerQuestionDetailsResult? Get([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return null;

        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser.UserId,
            _totalsPersUserLoader,
            extendedUserCache);

        var correctnessProbability =
            answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;
        var sessionUser = extendedUserCache.GetItem(_sessionUser.UserId);
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? sessionUser.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation =
            userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;
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
                new AnswerQuestionDetailsTopic(
                    Id: t.Id,
                    Name: t.Name,
                    Url: new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(
                        t.Name, t.Id),
                    QuestionCount: t.GetCountQuestionsAggregated(_sessionUser.UserId),
                    ImageUrl: new CategoryImageSettings(t.Id, _httpContextAccessor)
                        .GetUrl_128px(asSquare: true).Url,
                    IconHtml: CategoryCachedData.GetIconHtml(t),
                    MiniImageUrl: new ImageFrontendData(
                            _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Category),
                            _httpContextAccessor,
                            _questionReadingRepo)
                        .GetImageUrl(30, true, false, ImageType.Category).Url,
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
            TotalViewCount: question.TotalViews,
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