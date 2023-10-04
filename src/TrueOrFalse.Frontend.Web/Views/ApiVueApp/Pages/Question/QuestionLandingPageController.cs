using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Web;

namespace VueApp;
public class QuestionLandingPageController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        TotalsPersUserLoader totalsPersUserLoader,
        SessionUserCache sessionUserCache,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        QuestionReadingRepo questionReadingRepo)
    : Controller
{
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

        if (!permissionCheck.CanView(q))
        {
            throw new SecurityException("Not allowed to view question");
        }

        var primaryTopic = q.Categories.LastOrDefault();
        var solution = GetQuestionSolution.Run(q);
        var title = Regex.Replace(q.Text, "<.*?>", String.Empty);
        EscapeReferencesText(q.References);
        return Json(new
        {
            answerBodyModel = new
            {
                id = q.Id,
                text = q.Text,
                title = title,
                solutionType = q.SolutionType,
                renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
                description = q.Description,
                hasTopics = q.Categories.Any(),
                primaryTopicId = primaryTopic?.Id,
                primaryTopicName = primaryTopic?.Name,
                solution = q.Solution,

                isCreator = q.Creator.Id == sessionUser.UserId,
                isInWishknowledge = sessionUser.IsLoggedIn && q.IsInWishknowledge(sessionUser.UserId, sessionUserCache),

                questionViewGuid = Guid.NewGuid(),
                isLastStep = true,
                imgUrl = GetQuestionImageFrontendData.Run(q, 
                    imageMetaDataReadingRepo, 
                    httpContextAccessor, 
                    webHostEnvironment, 
                   questionReadingRepo)
                    .GetImageUrl(435, true, imageTypeForDummy: ImageType.Question)
                    .Url
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
            answerQuestionDetailsModel = new AnswerQuestionDetailsController(sessionUser,
                permissionCheck, 
                imageMetaDataReadingRepo, 
                totalsPersUserLoader,
                httpContextAccessor,
                webHostEnvironment, 
                sessionUserCache,
                actionContextAccessor,
                questionReadingRepo)
                .GetData(id)

        });
    }

}