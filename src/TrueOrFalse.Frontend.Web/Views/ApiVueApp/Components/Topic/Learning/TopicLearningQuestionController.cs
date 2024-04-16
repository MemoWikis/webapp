using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class TopicLearningQuestionController(
    SessionUser _sessionUser,
    CommentRepository _commentRepository,
    TotalsPersUserLoader _totalsPersUserLoader,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    SessionUserCache _sessionUserCache) : Controller
{
    public readonly record struct QuestionDataResult(bool Success, QuestionData Data);

    public readonly record struct QuestionData(
        string Answer,
        string ExtendedAnswer,
        string AuthorName,
        int AuthorId,
        string AuthorImageUrl,
        string ExtendedQuestion,
        int CommentCount,
        bool IsCreator,
        int AnswerCount,
        int CorrectAnswerCount,
        int WrongAnswerCount,
        bool CanBeEdited,
        string Title,
        QuestionVisibility Visibility);

    [HttpPost]
    public QuestionDataResult LoadQuestionData([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id, _httpContextAccessor)
            .GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser.UserId,
            _totalsPersUserLoader,
            _sessionUserCache);
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var result = new QuestionDataResult
        {
            Success = true,
            Data = new QuestionData
            {
                Answer = solution.GetCorrectAnswerAsHtml(),
                ExtendedAnswer = question.DescriptionHtml ?? "",
                AuthorName = author.Name,
                AuthorId = author.Id,
                AuthorImageUrl = authorImage.Url,
                ExtendedQuestion = question.TextExtendedHtml ?? "",
                CommentCount = _commentRepository.GetForDisplay(question.Id)
                    .Where(c => !c.IsSettled)
                    .Select(c => new CommentModel(c, _httpContextAccessor, _webHostEnvironment))
                    .ToList()
                    .Count(),
                IsCreator = author.Id == _sessionUser.UserId,
                AnswerCount = history.TimesAnsweredUser,
                CorrectAnswerCount = history.TimesAnsweredUserTrue,
                WrongAnswerCount = history.TimesAnsweredUserWrong,
                CanBeEdited = question.Creator?.Id == _sessionUser.UserId ||
                              _sessionUser.IsInstallationAdmin,
                Title = question.Text,
                Visibility = question.Visibility
            }
        };

        return result;
    }

    [HttpGet]
    public KnowledgeStatus GetKnowledgeStatus([FromRoute] int id)
    {
        var sessionUser = _sessionUserCache.GetItem(_sessionUser.UserId);
        if (sessionUser == null)
        {
            throw new NullReferenceException("sessionUser can't null");
        }

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? sessionUser.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(id) && _sessionUser.IsLoggedIn;

        return hasUserValuation
            ? userQuestionValuation[id].KnowledgeStatus
            : KnowledgeStatus.NotLearned;
    }
}