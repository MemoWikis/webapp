﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;
public class QuestionLandingPageController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;

    public QuestionLandingPageController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationRepo categoryValuationRepo,
        ImageMetaDataRepo imageMetaDataRepo) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _categoryValuationRepo = categoryValuationRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
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
        var title = Regex.Replace(q.Text, "<.*?>", String.Empty);
        EscapeReferencesText(q.References);
        return Json(new
        {
            answerBodyModel = new
            {
                id = q.Id,
                text = q.Text,
                title = title,
                encodedTitle = UriSanitizer.Run(title, 10),
                solutionType = q.SolutionType,
                renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
                description = q.Description,
                hasTopics = q.Categories.Any(),
                primaryTopicUrl = "/" + UriSanitizer.Run(primaryTopic?.Name) + "/" + primaryTopic?.Id,
                primaryTopicName = primaryTopic?.Name,
                solution = q.Solution,

                isCreator = q.Creator.Id = _sessionUser.UserId,
                isInWishknowledge = _sessionUser.IsLoggedIn && q.IsInWishknowledge(_sessionUser.UserId, _categoryValuationRepo),

                questionViewGuid = Guid.NewGuid(),
                isLastStep = true,
                imgUrl = GetQuestionImageFrontendData.Run(q).GetImageUrl(435, true, imageTypeForDummy: ImageType.Question).Url
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
            answerQuestionDetailsModel = new AnswerQuestionDetailsController(_sessionUser,_permissionCheck, _categoryValuationRepo, _imageMetaDataRepo).GetData(id)

        }, JsonRequestBehavior.AllowGet);
    }

}