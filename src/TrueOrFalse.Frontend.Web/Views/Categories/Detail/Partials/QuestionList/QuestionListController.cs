using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListController : BaseController
{
    public int CategoryId;
    public int AllQuestionCount;
    public List<Question> AllQuestions;
    public ConcurrentDictionary<int, QuestionValuation> UserQuestionValuation { get; set; }


    public QuestionListController(int categoryId)
    {
        CategoryId = categoryId;
        AllQuestions = EntityCache.GetQuestionsForCategory(CategoryId).ToList();
        AllQuestionCount = AllQuestions.Count();
        if (IsLoggedIn)
        {
            var user = Sl.R<SessionUser>().User;
            UserQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;
        }
    }

    [HttpPost]
    public int GetPageCount(int itemCountPerPage)
    {
        var pageCount = AllQuestionCount / itemCountPerPage;
        return pageCount;
    }

    [HttpPost]
    public JsonResult LoadQuestions(int itemCount, int pageNumber, string sortCondition, string filterCondition)
    {
        var questions = AllQuestions.ToList();
        if (IsLoggedIn)
        {
        }
        var questionsList = new List<QuestionListJson.Question>();

        var skimmedQuestions = questions.Skip(itemCount * (pageNumber - 1)).Take(itemCount).ToList();

        foreach (Question q in skimmedQuestions)
        {
            var questionObj = new QuestionListJson.Question();
            var userTinyModel = new UserTinyModel(q.Creator);
            questionObj.Title = q.Text;
            questionObj.LinkToQuestion = Links.GetUrl(q);
            questionObj.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(30);
            if (IsLoggedIn)
            {
                if (UserQuestionValuation.ContainsKey(q.Id))
                {
                    questionObj.CorrectnessProbability = UserQuestionValuation[q.Id].CorrectnessProbability;
                    questionObj.IsInWishknowledge = UserQuestionValuation[q.Id].IsInWishKnowledge();
                } 
            }
            else
                questionObj.CorrectnessProbability = q.CorrectnessProbability;

            questionObj.Author.Name = userTinyModel.Name;
            questionObj.Author.ImageData = new UserImageSettings(userTinyModel.Id).GetUrl_128px_square(userTinyModel);
            questionObj.Author.Id = userTinyModel.Id;
        }

        return Json(new QuestionListJson()
        {
            
        });
    }

    [HttpPost]
    public JsonResult LoadQuestionBody(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var userTinyModel = new UserTinyModel(question.Creator);


        return Json(new
        {
            answer = question.Solution,
            extendedAnswer = question.ExtendedAnswer,
            categories = question.Categories, 
            sources = question.SourceList,
        });
    }
}