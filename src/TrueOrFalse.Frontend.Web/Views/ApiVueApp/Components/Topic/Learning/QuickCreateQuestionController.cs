using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    public readonly record struct CreateFlashcardRequest(
        int TopicId,
        string TextHtml,
        string Answer,
        int Visibility,
        bool AddToWishknowledge,
        int LastIndex,
        LearningSessionConfig SessionConfig,
        string[] UploadedImagesMarkedForDeletion,
        string[] UploadedImagesInContent
    );

    public readonly record struct CreateFlashcardResponse(bool Success, int Data, string MessageKey);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateFlashcardResponse CreateFlashcard([FromBody] CreateFlashcardRequest request)
    {
        var safeText = GetSafeText(request.TextHtml);

        if (string.IsNullOrEmpty(safeText))
        {
            return new CreateFlashcardResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingText
            };
        }

        var question = new Question
        {
            TextHtml = request.TextHtml,
            Text = safeText,
            SolutionType = SolutionType.FlashCard
        };

        var solutionModelFlashCard = new QuestionSolutionFlashCard
        {
            Text = request.Answer
        };

        if (string.IsNullOrEmpty(solutionModelFlashCard.Text))
        {
            return new CreateFlashcardResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Question.MissingAnswer
            };
        }

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId)!;

        question.Categories = new List<Category>
        {
            _categoryRepository.GetById(request.TopicId)
        };

        var visibility = (QuestionVisibility)request.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (request.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        if (request.UploadedImagesInContent.Length > 0)
            SaveImageToFile.ReplaceTempQuestionContentImages(request.UploadedImagesInContent, question, _questionWritingRepo);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        _learningSessionCreator.InsertNewQuestionToLearningSession(
            questionCacheItem,
            request.LastIndex,
            request.SessionConfig);

        return new CreateFlashcardResponse
        {
            Success = true,
            Data = new QuestionLoader(
                _sessionUser,
                _extendedUserCache,
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