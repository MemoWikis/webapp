﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;

namespace VueApp;
public class QuickCreateQuestionController : BaseController
{
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

        question.Creator = Sl.UserRepo.GetById(SessionUser.UserId);
        question.Categories = new List<Category>
        {
            Sl.CategoryRepo.GetById(flashCardJson.TopicId)
        };
        var visibility = (QuestionVisibility)flashCardJson.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        var questionRepo = Sl.QuestionRepo;
        questionRepo.Create(question);

        if (flashCardJson.AddToWishknowledge)
            QuestionInKnowledge.Pin(Convert.ToInt32(question.Id), SessionUser.UserId);

        LearningSessionCache.InsertNewQuestionToLearningSession(EntityCache.GetQuestion(question.Id), flashCardJson.LastIndex, flashCardJson.SessionConfig);
        var questionController = new QuestionController(questionRepo);

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