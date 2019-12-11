using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListController : BaseController
{
    public int CategoryId;
    public int AllQuestionCount;

    public QuestionListController(int categoryId, int allQuestionCount)
    {
        CategoryId = categoryId;
        AllQuestionCount = allQuestionCount;
    }

    [HttpPost]
    public int GetPageCount(int categoryId, int itemCountPerPage)
    {
        return 0;
    }

    [HttpPost]
    public JsonResult LoadQuestions(int categoryId, int pageNumber, string sortCondition, string filterCondition)
    {
        var questions = EntityCache.GetQuestionsForCategory(categoryId);
        var questionsList = new List<KnowledgeQuestions.Questions>();


        return Json(new QuestionListJson()
        {
            
        });
    }

    [HttpPost]
    public JsonResult LoadQuestionBody(int questionId)
    {
        var question = new QuestionListJson.Question();

        return Json(new
        {
            answer = question.Answer,
            extendedAnswer = question.ExtendedAnswer,
            categories = question.CategoryList, 
            author = question.AuthorList,
            sources = question.SourceList,
        });
    }
}