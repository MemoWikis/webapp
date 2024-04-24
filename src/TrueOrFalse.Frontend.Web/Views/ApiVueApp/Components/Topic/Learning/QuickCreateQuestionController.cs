using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using TrueOrFalse;

namespace VueApp;

public class QuickCreateQuestionController(
    SessionUser _sessionUser,
    LearningSessionCreator _learningSessionCreator,
    QuestionInKnowledge _questionInKnowledge,
    LearningSessionCache _learningSessionCache,
    CategoryRepository _categoryRepository,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    UserReadingRepo _userReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    SessionUserCache _sessionUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    public readonly record struct CreateFlashcardParam(
        int TopicId,
        string TextHtml,
        string Answer,
        int Visibility,
        bool AddToWishknowledge,
        int LastIndex,
        LearningSessionConfig SessionConfig
    );

    public readonly record struct CreateFlashcardResult(bool Success, int Data, string MessageKey);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateFlashcardResult CreateFlashcard([FromBody] CreateFlashcardParam param)
    {
        var safeText = GetSafeText(param.TextHtml);
        if (safeText.Length <= 0)
            return new CreateFlashcardResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingText
            };

        var question = new Question();

        question.TextHtml = param.TextHtml;
        question.Text = safeText;
        question.SolutionType = SolutionType.FlashCard;

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = param.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return new CreateFlashcardResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingAnswer
            };

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId);
        question.Categories = new List<Category>
        {
            _categoryRepository.GetById(param.TopicId)
        };
        var visibility = (QuestionVisibility)param.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (param.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        _learningSessionCreator.InsertNewQuestionToLearningSession(
            EntityCache.GetQuestion(question.Id), param.LastIndex, param.SessionConfig);

        return new CreateFlashcardResult
        {
            Success = true,
            Data = new QuestionLoader(
                _sessionUser,
                _sessionUserCache,
                _httpContextAccessor,
                _actionContextAccessor,
                _imageMetaDataReadingRepo,
                _questionReadingRepo,
                _learningSessionCache).LoadQuestion(question.Id).SessionIndex
        };
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }
}