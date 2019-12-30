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

public class QuestionListController : BaseController
{
    public int CategoryId;
    public int AllQuestionCount;
    public List<Question> AllQuestions;
    public ConcurrentDictionary<int, QuestionValuation> UserQuestionValuations { get; set; }


    public QuestionListController(int categoryId)
    {
        CategoryId = categoryId;
        AllQuestions = EntityCache.GetQuestionsForCategory(CategoryId).ToList();
        AllQuestionCount = AllQuestions.Count();
        if (IsLoggedIn)
        {
            var user = Sl.R<SessionUser>().User;
            UserQuestionValuations = UserCache.GetItem(user.Id).QuestionValuations;
        }
    }

    [HttpPost]
    public int GetPageCount(int itemCountPerPage)
    {
        var pageCount = AllQuestionCount / itemCountPerPage;
        return pageCount;
    }

    [HttpPost]
    public JsonResult LoadQuestions(int categoryId, int itemCount, int pageNumber, string sortCondition, string filterCondition)
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

        return Json(new
        {
            answer = question.Solution,
            extendedAnswer = question.Description,
            categories = question.Categories, 
            references = question.References,
            author = author,
            authorImage = authorImage,
        });
    }
}