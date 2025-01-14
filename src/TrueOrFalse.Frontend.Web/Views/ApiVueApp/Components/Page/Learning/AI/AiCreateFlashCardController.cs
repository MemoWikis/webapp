using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TrueOrFalse;

namespace VueApp;

public class AiCreateFlashCardController(
    SessionUser _sessionUser,
    QuestionInKnowledge _questionInKnowledge,
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    Logg _logg,
    LearningSessionCreator _learningSessionCreator) : Controller
{
    public readonly record struct FlashCardJson(string front, string back);

    public readonly record struct CreateRequest(
        int PageId,
        FlashCardJson[] flashcards,
        int LastIndex,
        LearningSessionConfig SessionConfig);

    public readonly record struct CreateResponse(bool Success, int[]? Ids = null, string? MessageKey = null, int? LastIndex = null);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateResponse Create([FromBody] CreateRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
            return new CreateResponse(false);

        var ids = new List<int>();

        var lastIndex = request.LastIndex;

        foreach (var flashcardJson in request.flashcards)
        {
            var id = CreateFlashCard(flashcardJson, request.PageId);
            if (id != null && id > 0)
            {
                ids.Add((int)id);

                var questionCacheItem = EntityCache.GetQuestion((int)id);

                _learningSessionCreator.InsertNewQuestionToLearningSession(
                    questionCacheItem,
                    request.LastIndex,
                    request.SessionConfig);

                lastIndex++;
            }
        }

        return new CreateResponse
        {
            Success = true,
            Ids = ids.ToArray(),
            LastIndex = lastIndex
        };
    }

    private int? CreateFlashCard(FlashCardJson json, int pageId)
    {
        var safeText = GetSafeText(json.front);

        if (string.IsNullOrEmpty(safeText))
        {
            return null;
        }

        var question = new Question
        {
            TextHtml = json.front,
            Text = safeText,
            SolutionType = SolutionType.FlashCard
        };

        var solutionModelFlashCard = new QuestionSolutionFlashCard
        {
            Text = json.back
        };

        if (string.IsNullOrEmpty(solutionModelFlashCard.Text))
        {
            return null;
        }

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId)!;

        question.Pages = new List<Page>
        {
            pageRepository.GetById(pageId)
        };

        question.Visibility = new LimitCheck(_logg, _sessionUser).CanSavePrivateQuestion() ? QuestionVisibility.Owner : QuestionVisibility.All;

        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, pageRepository);
        _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return question.Id;
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }
}