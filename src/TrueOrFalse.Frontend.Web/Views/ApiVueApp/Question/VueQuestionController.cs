﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class VueQuestionController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly RestoreQuestion _restoreQuestion;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly TotalsPersUserLoader _totalsPersUserLoader;

    public VueQuestionController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        RestoreQuestion restoreQuestion,
        LearningSessionCache learningSessionCache,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        QuestionReadingRepo questionReadingRepo,
        TotalsPersUserLoader totalsPersUserLoader) 
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _restoreQuestion = restoreQuestion;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _questionReadingRepo = questionReadingRepo;
        _totalsPersUserLoader = totalsPersUserLoader;
    }

    [HttpGet]
    public JsonResult GetQuestion(int id)
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
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(new { }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetQuestionPage(int id)
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
                primaryTopicUrl = "/" + UriSanitizer.Run(primaryTopic?.Name) + "/" + primaryTopic?.Id,
                primaryTopicName = primaryTopic?.Name,
                solution = q.Solution,

                isCreator = q.Creator.Id == _sessionUser.UserId,
                isInWishknowledge = _sessionUser.IsLoggedIn && q.IsInWishknowledge(_sessionUser.UserId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo),

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
            answerQuestionDetailsModel = new AnswerQuestionDetailsController(_sessionUser,_permissionCheck, _categoryValuationReadingRepo, _imageMetaDataReadingRepo, _userReadingRepo, _questionValuationRepo, _totalsPersUserLoader).GetData(id)
        }, JsonRequestBehavior.AllowGet);
    }


    public JsonResult LoadQuestion(int questionId)
    {
        var userQuestionValuation = _sessionUser.IsLoggedIn ? 
            SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo)
                .QuestionValuations : null;

        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question))
            .GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
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
        return Redirect(Links.AnswerQuestion(question));
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