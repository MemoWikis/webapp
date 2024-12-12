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
    QuestionWritingRepo _questionWritingRepo) : Controller
{
    public readonly record struct CreateRequest(
        int PageId,
        string Front,
        string Back
    );

    public readonly record struct CreateResponse(bool Success, int? Id = null, string? MessageKey = null);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateResponse Create([FromBody] CreateRequest request)
    {
        var safeText = GetSafeText(request.Front);

        if (string.IsNullOrEmpty(safeText))
        {
            return new CreateResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingText
            };
        }

        var question = new Question
        {
            TextHtml = request.Front,
            Text = safeText,
            SolutionType = SolutionType.FlashCard
        };

        var solutionModelFlashCard = new QuestionSolutionFlashCard
        {
            Text = request.Back
        };

        if (string.IsNullOrEmpty(solutionModelFlashCard.Text))
        {
            return new CreateResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingAnswer
            };
        }

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId)!;

        question.Pages = new List<Page>
        {
            pageRepository.GetById(request.PageId)
        };

        question.Visibility = QuestionVisibility.Owner;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, pageRepository);
        _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return new CreateResponse
        {
            Success = true,
            Id = question.Id,
        };
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }
}