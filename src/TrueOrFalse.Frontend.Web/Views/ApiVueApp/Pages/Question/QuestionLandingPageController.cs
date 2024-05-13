using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse;
using TrueOrFalse.Domain.Question.Answer;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class QuestionLandingPageController(
    TotalsPersUserLoader _totalsPersUserLoader,
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    ExtendedUserCache extendedUserCache,
    IActionContextAccessor _actionContextAccessor,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    private static void EscapeReferencesText(IList<ReferenceCacheItem> references)
    {
        foreach (var reference in references)
        {
            if (reference.ReferenceText != null)
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>")
                    .Replace("\\n", "<br/>");
            if (reference.AdditionalInfo != null)
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>")
                    .Replace("\\n", "<br/>");
        }
    }

    public readonly record struct QuestionPageResult(
        AnswerBodyModel AnswerBodyModel,
        SolutionData SolutionData,
        AnswerQuestionDetailsResult? AnswerQuestionDetailsModel);

    public readonly record struct AnswerBodyModel(
        int Id,
        string Text,
        string Title,
        SolutionType SolutionType,
        string RenderedQuestionTextExtended,
        string Description,
        bool HasTopics,
        int? PrimaryTopicId,
        string PrimaryTopicName,
        string Solution,
        bool IsCreator,
        bool IsInWishknowledge,
        Guid questionViewGuid,
        bool IsLastStep,
        string ImgUrl);

    public readonly record struct SolutionData(
        string AnswerAsHTML,
        string Answer,
        string AnswerDescription,
        AnswerReference[] AnswerReferences);

    public readonly record struct AnswerReference(
        int ReferenceId,
        int? TopicId,
        string ReferenceType,
        string AdditionalInfo,
        string ReferenceText);

    [HttpGet]
    public QuestionPageResult GetQuestionPage([FromRoute] int id)
    {
        var q = EntityCache.GetQuestion(id);

        if (!_permissionCheck.CanView(q))
        {
            throw new SecurityException("Not allowed to view question");
        }

        var primaryTopic = q.Categories.LastOrDefault();
        var solution = GetQuestionSolution.Run(q);
        var title = Regex.Replace(q.Text, "<.*?>", String.Empty);
        EscapeReferencesText(q.References);
        return new QuestionPageResult
        {
            AnswerBodyModel = new AnswerBodyModel
            {
                Id = q.Id,
                Text = q.Text,
                Title = title,
                SolutionType = q.SolutionType,
                RenderedQuestionTextExtended = q.TextExtended != null
                    ? MarkdownMarkdig.ToHtml(q.TextExtended)
                    : "",
                Description = q.Description,
                HasTopics = q.Categories.Any(),
                PrimaryTopicId = primaryTopic?.Id,
                PrimaryTopicName = primaryTopic?.Name,
                Solution = q.Solution,
                IsCreator = q.Creator.Id == _sessionUser.UserId,
                IsInWishknowledge = _sessionUser.IsLoggedIn &&
                                    q.IsInWishknowledge(_sessionUser.UserId, extendedUserCache),
                questionViewGuid = Guid.NewGuid(),
                IsLastStep = true,
                ImgUrl = GetQuestionImageFrontendData.Run(q,
                        _imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetImageUrl(435, true, imageTypeForDummy: ImageType.Question)
                    .Url
            },
            SolutionData = new SolutionData
            {
                AnswerAsHTML = solution.GetCorrectAnswerAsHtml(),
                Answer = solution.CorrectAnswer(),
                AnswerDescription =
                    q.Description != null ? MarkdownMarkdig.ToHtml(q.Description) : "",
                AnswerReferences = q.References.Select(r => new AnswerReference
                {
                    ReferenceId = r.Id,
                    TopicId = r.Category?.Id,
                    ReferenceType = r.ReferenceType.GetName(),
                    AdditionalInfo = r.AdditionalInfo ?? "",
                    ReferenceText = r.ReferenceText ?? ""
                }).ToArray()
            },
            AnswerQuestionDetailsModel =
                GetData(id)
        };
    }

    private AnswerQuestionDetailsResult? GetData(int id)
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

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? extendedUserCache.GetItem(_sessionUser.UserId).QuestionValuations
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