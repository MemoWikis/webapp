using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MarkdownSharp;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RabbitMQ.Client.Framing.Impl;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class QuestionListController : BaseController
{
    [HttpPost]
    public JsonResult LoadQuestions(int categoryId, int itemCount, int pageNumber)
    {
        var newQuestionList = QuestionListModel.PopulateQuestionsOnPage(categoryId, pageNumber, itemCount, IsLoggedIn);
        return Json(newQuestionList);
    }

    [HttpPost]
    public JsonResult LoadQuestionBody(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id).GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);

        var json = Json(new
        {
            answer = solution.GetCorrectAnswerAsHtml(),
            extendedAnswer = question.Description != null ? MarkdownMarkdig.ToHtml(question.Description) : "",
            categories = question.Categories.Select(c => new
            {
                name = c.Name,
                categoryType = c.Type,
                linkToCategory = Links.CategoryDetail(c),
            }),
            references = question.References.Select(r => new
            {
                referenceType = r.ReferenceType.GetName(),
                additionalInfo = r.AdditionalInfo ?? "",
                referenceText = r.ReferenceText ?? ""
            }).AsEnumerable().Distinct().ToList(),
            author = author.Name,
            authorId = author.Id,
            authorImage = authorImage.Url,
            authorUrl = Links.UserDetail(author),
            extendedQuestion = question.TextExtended != null ? MarkdownMarkdig.ToHtml(question.TextExtended) : "",
            commentCount = Resolve<CommentRepository>().GetForDisplay(question.Id)
                .Where(c => !c.IsSettled)
                .Select(c => new CommentModel(c))
                .ToList()
                .Count(),
            isCreator = author.Id == _sessionUser.UserId,
            editUrl = Links.EditQuestion(Url, question.Text, question.Id),
            historyUrl = Links.QuestionHistory(question.Id)
        });

        return json;
    }

    [HttpPost]
    public string RenderWishknowledgePinButton(bool isInWishknowledge)
    {
        return ViewRenderer.RenderPartialView("~/Views/Shared/AddToWishknowledgeButtonQuestionDetail.ascx", new AddToWishknowledge(isInWishknowledge, true), ControllerContext);
    }

    [HttpPost]
    public int GetUpdatedCorrectnessProbability(int questionId)
    {
        var question = Sl.QuestionRepo.GetById(questionId);
        var model = new AnswerQuestionModel(question);

        return model.HistoryAndProbability.CorrectnessProbability.CPPersonal;
    }
}