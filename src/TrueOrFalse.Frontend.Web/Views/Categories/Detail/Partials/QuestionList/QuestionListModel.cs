﻿using System;
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
    public IList<Question> AllQuestions;
    public List<QuestionValuation> QuestionsOnPage;
    public ConcurrentDictionary<int, QuestionValuation> UserQuestionValuation { get; set; }
    public int CurrentPage;
    public int ItemCount;
    public string QuestionsOnFirstPage;


    public QuestionListModel(int categoryId)
    {
        CategoryId = categoryId;
        AllQuestions = GetAllQuestions(categoryId);
        AllQuestionCount = AllQuestions.Count();
        var json = new JavaScriptSerializer();
        var questionsOnFirstPage = PopulateQuestionsOnPage(CategoryId, 25, 1, IsLoggedIn);

        QuestionsOnFirstPage = json.Serialize(questionsOnFirstPage);
    }

    public static IList<Question> GetAllQuestions(int categoryId)
    {
        return EntityCache.GetQuestionsForCategory(categoryId);
    }

    public static List<QuestionListJson.Question> PopulateQuestionsOnPage(int categoryId, int currentPage, int itemCount, bool isLoggedIn)
    {
        var allQuestions = GetAllQuestions(categoryId);

        var questionsOfCurrentPage = allQuestions.Skip(itemCount * (currentPage - 1)).Take(itemCount).ToList();
        var newQuestionList = new List<QuestionListJson.Question>();

        
        foreach (Question q in questionsOfCurrentPage)
        {
            var question = new QuestionListJson.Question();
            question.Title = q.Text;
            question.LinkToQuestion = Links.GetUrl(q);
            question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(30);

            if (isLoggedIn)
            {
                var user = Sl.R<SessionUser>().User;
                var userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations[q.Id];
                question.CorrectnessProbability = userQuestionValuation.CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation.IsInWishKnowledge();
            }
            else
                question.CorrectnessProbability = q.CorrectnessProbability;

            newQuestionList.Add(question);
        }

        return newQuestionList;
    }
}