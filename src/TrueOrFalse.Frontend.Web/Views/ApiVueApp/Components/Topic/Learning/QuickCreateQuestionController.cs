﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;

namespace VueApp;
public class QuickCreateQuestionController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly UserRepo _userRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;

    public QuickCreateQuestionController(SessionUser sessionUser,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        LearningSessionCache learningSessionCache,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CategoryRepository categoryRepository,
        ImageMetaDataRepo imageMetaDataRepo,
        UserRepo userRepo, 
        QuestionValuationRepo questionValuationRepo,
        QuestionWritingRepo questionWritingRepo)
    {
        _sessionUser = sessionUser;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _categoryRepository = categoryRepository;
        _imageMetaDataRepo = imageMetaDataRepo;
        _userRepo = userRepo;
        _questionValuationRepo = questionValuationRepo;
        _questionWritingRepo = questionWritingRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard(FlashCardLoader flashCardJson)
    {
        
        var safeText = GetSafeText(flashCardJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult
            {
                Data = new
                {
                    error = true,
                    key = "missingText",
                }
            };
        var serializer = new JavaScriptSerializer();
        var question = new Question();

        question.TextHtml = flashCardJson.TextHtml;
        question.Text = safeText;
        question.SolutionType = SolutionType.FlashCard;

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = flashCardJson.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return new JsonResult
            {
                Data = new
                {
                    error = true,
                    key = "missingAnswer",
                }
            };

        question.Solution = serializer.Serialize(solutionModelFlashCard);

        question.Creator = _userRepo.GetById(_sessionUser.UserId);
        question.Categories = new List<Category>
        {
            _categoryRepository.GetById(flashCardJson.TopicId)
        };
        var visibility = (QuestionVisibility)flashCardJson.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (flashCardJson.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        _learningSessionCreator.InsertNewQuestionToLearningSession(EntityCache.GetQuestion(question.Id), flashCardJson.LastIndex, flashCardJson.SessionConfig);
        var questionController = new QuestionController(_sessionUser,
            _learningSessionCache, 
            _categoryValuationReadingRepo,
            _imageMetaDataRepo, 
            _userRepo, 
            _questionValuationRepo); 

        return questionController.LoadQuestion(question.Id);
    }

    public class FlashCardLoader
    {
        public int TopicId { get; set; }
        public string TextHtml { get; set; }
        public string Answer { get; set; }
        public int Visibility { get; set; }
        public bool AddToWishknowledge { get; set; }
        public int LastIndex { get; set; }
        public LearningSessionConfig SessionConfig { get; set; }
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }
}