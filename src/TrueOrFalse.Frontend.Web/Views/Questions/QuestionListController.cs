using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, questionId);
        var solution = GetQuestionSolution.Run(question);

        var json = Json(new
        {
            answer = solution.CorrectAnswer(),
            extendedAnswer = question.Description,
            categories = question.Categories.Select(c => new
            {
                name = c.Name,
                categoryType = c.Type,
                url = c.Url,
            }),
            references = question.References.Select(r => new
            {
                referenceType = r.ReferenceType.GetName(),
                additionalInfo = r.AdditionalInfo ?? "",
                referenceText = r.ReferenceText ?? ""
            }),
            author = author.Name,
            authorId = author.Id,
            authorImage = authorImage.Url,
            questionDetails = new {
                extendedQuestion = question.TextExtended,
                views = question.TotalViews,
                totalAnswers = question.TotalAnswers(),
                totalCorrectAnswers = question.TotalTrueAnswers,
                personalAnswers = valuationForUser.Total(),
                personalCorrectAnswer = valuationForUser.TotalTrue
            },
        });

        return json;
    }
}