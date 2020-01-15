using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel : BaseModel
{
    public int CategoryId;
    public int AllQuestionCount;
    public int CurrentPage;
    public int ItemCount;


    public QuestionListModel(int categoryId)
    {
        CategoryId = categoryId;
        var questionList = GetAllQuestions(categoryId);
        AllQuestionCount = questionList.Count();
    }

    public static IList<Question> GetAllQuestions(int categoryId)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        return category.GetAggregatedQuestionsFromMemoryCache();
    }

    public static List<QuestionListJson.Question> PopulateQuestionsOnPage(int categoryId, int currentPage, int itemCount, bool isLoggedIn)
    {
        var allQuestions = GetAllQuestions(categoryId);
        var questionsOfCurrentPage = allQuestions.Skip(itemCount * (currentPage - 1)).Take(itemCount).ToList();
        var newQuestionList = new List<QuestionListJson.Question>();

        foreach (Question q in questionsOfCurrentPage)
        {
            var question = new QuestionListJson.Question();
            question.Id = q.Id;
            question.Title = q.Text;
            question.LinkToQuestion = Links.GetUrl(q);
            question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(30).Url;

            question.CorrectnessProbability = q.CorrectnessProbability;

            if (isLoggedIn)
            {
                var user = Sl.R<SessionUser>().User;
                var userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;
                if (userQuestionValuation.ContainsKey(q.Id))
                {
                    question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                    question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge();
                    question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
                }
            }

            newQuestionList.Add(question);
        }

        return newQuestionList;
    }


}