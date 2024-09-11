using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrueOrFalse;
using TrueOrFalse.Domain.Question.Answer;
using TrueOrFalse.Web;

namespace VueApp;

public class QuestionLandingPageController(
    TotalsPersUserLoader _totalsPersUserLoader,
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    ExtendedUserCache _extendedUserCache,
    SaveQuestionView _saveQuestionView,
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
        AnswerQuestionDetailsResult? AnswerQuestionDetailsModel,
        string MessageKey);

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
        Guid QuestionViewGuid,
        bool IsLastStep,
        string ImgUrl,
        string TextHtml);

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
        var question = EntityCache.GetQuestion(id);
        if (question == null)
        {
            Logg.r.Warning($"questions: Not found the question in function {nameof(GetQuestionPage)}");
            return new QuestionPageResult
            {
                MessageKey = FrontendMessageKeys.Error.Question.NotFound
            };
        }

        var canView = _permissionCheck.CanView(question);
        if (_sessionUser.IsLoggedIn == false && canView == false)
        {
            Logg.r.Warning($"questions: Not allowed to view question in function {nameof(GetQuestionPage)}");
            return new QuestionPageResult
            {
                MessageKey = FrontendMessageKeys.Error.Question.Unauthorized
            };
        }

        if (canView == false)
        {
            Logg.r.Warning($"questions: Not allowed to view question in function {nameof(GetQuestionPage)}");
            return new QuestionPageResult
            {
                MessageKey = FrontendMessageKeys.Error.Question.NoRights
            };
        }

        var primaryTopic = question.Categories.LastOrDefault();
        var solution = GetQuestionSolution.Run(question);
        var title = Regex.Replace(question.Text, "<.*?>", string.Empty);
        EscapeReferencesText(question.References);

        _saveQuestionView.Run(question, _sessionUser.UserId);

        return new QuestionPageResult
        {
            AnswerBodyModel = new AnswerBodyModel
            {
                Id = question.Id,
                Text = question.Text,
                TextHtml = question.TextHtml,
                Title = title,
                SolutionType = question.SolutionType,
                RenderedQuestionTextExtended = question.TextExtended != null
                    ? MarkdownMarkdig.ToHtml(question.TextExtended)
                    : "",
                Description = question.Description,
                HasTopics = question.Categories.Any(),
                PrimaryTopicId = primaryTopic?.Id,
                PrimaryTopicName = primaryTopic?.Name,
                Solution = question.Solution,
                IsCreator = question.Creator.Id == _sessionUser.UserId,
                IsInWishknowledge = _sessionUser.IsLoggedIn &&
                                    question.IsInWishknowledge(_sessionUser.UserId, _extendedUserCache),
                QuestionViewGuid = Guid.NewGuid(),
                IsLastStep = true,
                ImgUrl = GetQuestionImageFrontendData.Run(question,
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
                AnswerDescription = question.Description != null
                    ? MarkdownMarkdig.ToHtml(question.Description)
                    : "",
                AnswerReferences = question.References.Select(r => new AnswerReference
                {
                    ReferenceId = r.Id,
                    TopicId = r.Category?.Id,
                    ReferenceType = r.ReferenceType.GetName(),
                    AdditionalInfo = r.AdditionalInfo ?? "",
                    ReferenceText = r.ReferenceText ?? ""
                }).ToArray()
            },
            AnswerQuestionDetailsModel =
                GetData(id),
            MessageKey = ""
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
            _extendedUserCache);

        var correctnessProbability =
            answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId).QuestionValuations
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
                new AnswerQuestionDetailsTopicItem(
                    Id: t.Id,
                    Name: t.Name,
                    QuestionCount: t.GetCountQuestionsAggregated(_sessionUser.UserId),
                    ImageUrl: new CategoryImageSettings(t.Id, _httpContextAccessor)
                        .GetUrl_128px(asSquare: true).Url,
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