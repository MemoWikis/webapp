using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;
public class VueQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public VueQuestionController(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    [HttpGet]
    public JsonResult GetQuestion(int id)
    {
        var q = EntityCache.GetQuestionById(id);
        if (PermissionCheck.CanView(q))
        {
            return Json(new
            {
                Id = id,
                Text = q.Text,
                TextExtended = q.TextExtended,
                SolutionType = q.SolutionType,
                Solution = GetQuestionSolution.Run(q).GetCorrectAnswerAsHtml(),
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(new { }, JsonRequestBehavior.AllowGet);
    }

    public JsonResult LoadQuestion(int questionId)
    {
        var userQuestionValuation = IsLoggedIn ? SessionUserCache.GetItem(SessionUser.UserId).QuestionValuations : null;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = LearningSessionCache.GetLearningSession();
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

    [HttpPost]
    public JsonResult GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var categoryController = new CategoryController();
        var solution = question.SolutionType == SolutionType.FlashCard ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml() : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(PermissionCheck.CanView).Distinct();

        var json = new JsonResult
        {
            Data = new
            {
                SolutionType = (int)question.SolutionType,
                Solution = solution,
                SolutionMetadataJson = question.SolutionMetadataJson,
                Text = question.TextHtml,
                TextExtended = question.TextExtendedHtml,
                TopicIds = topicsVisibleToCurrentUser.Select(c => c.Id).ToList(),
                DescriptionHtml = question.DescriptionHtml,
                Topics = topicsVisibleToCurrentUser.Select(c => categoryController.FillMiniCategoryItem(c)),
                LicenseId = question.LicenseId,
                Visibility = question.Visibility,
            }
        };

        return json;
    }

    [HttpPost]
    public JsonResult DeleteDetails(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        var canBeDeleted = QuestionDelete.CanBeDeleted(question.Creator.Id, question);

        return new JsonResult
        {
            Data = new
            {
                questionTitle = question.Text.TruncateAtWord(90),
                totalAnswers = question.TotalAnswers(),
                canNotBeDeleted = !canBeDeleted.Yes,
                wuwiCount = canBeDeleted.WuwiCount,
                hasRights = canBeDeleted.HasRights
            }
        };
    }

    [HttpPost]
    public JsonResult Delete(int questionId, int sessionIndex)
    {
        QuestionDelete.Run(questionId);
        LearningSessionCache.RemoveQuestionFromLearningSession(sessionIndex, questionId);
        return new JsonResult
        {
            Data = new
            {
                sessionIndex,
                questionId
            }
        };
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult Restore(int questionId, int questionChangeId)
    {
        RestoreQuestion.Run(questionChangeId, this.User_());

        var question = Sl.QuestionRepo.GetById(questionId);
        return Redirect(Links.AnswerQuestion(question));
    }

    private static void EscapeReferencesText(IList<ReferenceCacheItem> references)
    {
        foreach (var reference in references)
        {
            if (reference.ReferenceText != null)
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>").Replace("\\n", "<br/>");
            if (reference.AdditionalInfo != null)
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>").Replace("\\n", "<br/>");
        }
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
                title = Regex.Replace(q.Text, "<.*?>", String.Empty),
                solutionType = q.SolutionType,
                renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
                description = q.Description,
                hasTopics = q.Categories.Any(),
                primaryTopicUrl = "/" + UriSanitizer.Run(primaryTopic?.Name) + "/" + primaryTopic?.Id,
                primaryTopicName = primaryTopic?.Name,
                solution = q.Solution,

                isCreator = q.Creator.Id = SessionUser.UserId,
                isInWishknowledge = SessionUser.IsLoggedIn && q.IsInWishknowledge(),

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
            answerQuestionDetailsModel = new AnswerQuestionDetailsController().GetData(id)

        }, JsonRequestBehavior.AllowGet);
    }

}