using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Domain.Question.Answer;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class VueQuestionController(SessionUser _sessionUser,
PermissionCheck _permissionCheck,
RestoreQuestion _restoreQuestion,
LearningSessionCache _learningSessionCache,
ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
UserReadingRepo _userReadingRepo,
QuestionReadingRepo _questionReadingRepo,
SessionUserCache _sessionUserCache,
IHttpContextAccessor _httpContextAccessor,
IActionContextAccessor _actionContextAccessor,
TotalsPersUserLoader _totalsPersUserLoader) : Controller
{
    [HttpGet]
    public JsonResult GetQuestion([FromRoute] int id)
    {
        var q = EntityCache.GetQuestionById(id);
        if (_permissionCheck.CanView(q))
        {
            return Json(new
            {
                Id = id,
                q.Text,
                q.TextExtended,
                q.SolutionType,
                Solution = GetQuestionSolution.Run(q).GetCorrectAnswerAsHtml()
            });
        }

        return Json(new { });
    }

    [HttpGet]
    public JsonResult GetQuestionPage([FromRoute] int id)
    {
        var q = EntityCache.GetQuestion(id);
        var primaryTopic = q.Categories.LastOrDefault();
        var solution = GetQuestionSolution.Run(q);

        EscapeReferencesText(q.References);
        return Json(new
        {
            answerBodyModel = new
            {
                id = q.Id,
                text = q.Text,
                title = Regex.Replace(q.Text, "<.*?>", string.Empty),
                solutionType = q.SolutionType,
                renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
                description = q.Description,
                hasTopics = q.Categories.Any(),
                primaryTopicId = primaryTopic?.Id,
                primaryTopicName = primaryTopic?.Name,
                solution = q.Solution,  
                isCreator = q.Creator.Id == _sessionUser.UserId,
                isInWishknowledge = _sessionUser.IsLoggedIn && q.IsInWishknowledge(_sessionUser.UserId, _sessionUserCache),
                questionViewGuid = Guid.NewGuid(),
                isLastStep = true
            },
            solutionData = new
            {   
                answerAsHTML = solution.GetCorrectAnswerAsHtml(),
                answer = solution.CorrectAnswer(),
                answerDescription = q.Description != null ? MarkdownMarkdig.ToHtml(q.Description) : "",
                answerReferences = q.References.Select(r => new
                {
                    referenceId = r.Id,
                    topicId = r.Category?.Id ?? null,
                    referenceType = r.ReferenceType.GetName(),
                    additionalInfo = r.AdditionalInfo ?? "",
                    referenceText = r.ReferenceText ?? ""
                }).ToArray()
            },
            answerQuestionDetailsModel = 
                GetData(id)
        });
    }

     public AnswerQuestionDetailsResult? GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return null;

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
        var result = new AnswerQuestionDetailsResult(
            KnowledgeStatus: hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            PersonalProbability: correctnessProbability.CPPersonal,
            PersonalColor: correctnessProbability.CPPColor,
            AvgProbability: correctnessProbability.CPAll,
            PersonalAnswerCount: history.TimesAnsweredUser,
            PersonalAnsweredCorrectly: history.TimesAnsweredUserTrue,
            PersonalAnsweredWrongly: history.TimesAnsweredUserWrong,
            OverallAnswerCount: history.TimesAnsweredTotal,
            OverallAnsweredCorrectly: history.TimesAnsweredCorrect,
            OverallAnsweredWrongly: history.TimesAnsweredWrongTotal,
            IsInWishknowledge: answerQuestionModel.HistoryAndProbability.QuestionValuation.IsInWishKnowledge,
            Topics: question.CategoriesVisibleToCurrentUser(_permissionCheck).Select(t => new AnswerQuestionDetailsTopic(
                Id: t.Id,
                Name: t.Name,
                Url: new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(t.Name, t.Id),
                QuestionCount: t.GetCountQuestionsAggregated(_sessionUser.UserId),
                ImageUrl: new CategoryImageSettings(t.Id, _httpContextAccessor).GetUrl_128px(asSquare: true).Url,
                IconHtml: CategoryCachedData.GetIconHtml(t),
                MiniImageUrl: new ImageFrontendData(
                        _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Category),
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetImageUrl(30, true, false, ImageType.Category).Url,
                Visibility: (int)t.Visibility,
                IsSpoiler:  IsSpoilerCategory.Yes(t.Name, question)
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
            WishknowledgeCount:  question.TotalRelevancePersonalEntries,
            License: new License(
            
                IsDefault: question.License.IsDefault(),
                ShortText: question.License.DisplayTextShort,
                FullText: question.License.DisplayTextFull
            )
        );
        return result;
    }

    public JsonResult LoadQuestion(int questionId)
    {
        var userQuestionValuation = _sessionUser.IsLoggedIn ? 
            _sessionUserCache.GetItem(_sessionUser.UserId)
                .QuestionValuations : null;

        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        var links = new Links(_actionContextAccessor, _httpContextAccessor);
        question.LinkToQuestion = links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                _httpContextAccessor, 
                _questionReadingRepo)
            .GetImageUrl(40, true).Url;

        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;
        question.CreatorId = q.CreatorId;

        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation != null && userQuestionValuation.ContainsKey(q.Id))
        {
            question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return Json(question);
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult Restore(int questionId, int questionChangeId)
    {
        _restoreQuestion.Run(questionChangeId, _userReadingRepo.GetById(_sessionUser.UserId));

        var question = _questionReadingRepo.GetById(questionId);
        return Redirect(new Links(_actionContextAccessor, _httpContextAccessor)
            .AnswerQuestion(question));
    }

    private static void EscapeReferencesText(IList<ReferenceCacheItem> references)
    {
        foreach (var reference in references)
        {
            if (reference.ReferenceText != null)
            {
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>").Replace("\\n", "<br/>");
            }

            if (reference.AdditionalInfo != null)
            {
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>").Replace("\\n", "<br/>");
            }
        }
    }
}