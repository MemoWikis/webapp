﻿using System.Collections.Concurrent;

public class PageLearningQuestionController(
    SessionUser _sessionUser,
    CommentRepository _commentRepository,
    TotalsPerUserLoader totalsPerUserLoader,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _sessionUserCache) : ApiBaseController
{
    public readonly record struct LoadQuestionDataResult(bool Success, QuestionData Data);

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
    public LoadQuestionDataResult LoadQuestionData([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id, _httpContextAccessor)
            .GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser,
            totalsPerUserLoader,
            _sessionUserCache);
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var result = new LoadQuestionDataResult
        {
            Success = true,
            Data = new QuestionData
            {
                Answer = solution.GetCorrectAnswerAsHtml(),
                ExtendedAnswer = question.DescriptionHtml ?? "",
                AuthorName = author.Name,
                AuthorId = author.Id,
                AuthorImageUrl = authorImage.Url,
                ExtendedQuestion = question.GetRenderedQuestionTextExtended(),
                CommentCount = _commentRepository.GetForDisplay(question.Id)
                    .Count(c => !c.IsSettled),
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
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(id) && _sessionUser.IsLoggedIn;

        return hasUserValuation
            ? userQuestionValuation[id].KnowledgeStatus
            : KnowledgeStatus.NotLearned;
    }
}