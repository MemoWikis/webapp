using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel : BaseModel
{
    public int CategoryId;
    public int AllQuestionCount;
    public List<Question> AllQuestions;
    public List<QuestionValuation> QuestionsOnPage;
    public ConcurrentDictionary<int, QuestionValuation> UserQuestionValuation { get; set; }
    public int CurrentPage;
    public int ItemCount;

    public QuestionListModel(int categoryId)
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

    private void PopulateQuestionsOnPage()
    {
        var questionsOfCurrentPage = AllQuestions.Skip(ItemCount * (CurrentPage - 1)).Take(ItemCount).ToList();

        foreach (Question q in questionsOfCurrentPage)
        {
            var question = new QuestionListJson.Question();
            question.Title = q.Text;
            question.LinkToQuestion = Links.GetUrl(q);
            question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(30);

            if (IsLoggedIn)
            {
                var user = Sl.R<SessionUser>().User;
                var userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations[q.Id];
                question.CorrectnessProbability = userQuestionValuation.CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation.IsInWishKnowledge();
            }
            else
                question.CorrectnessProbability = q.CorrectnessProbability;

        }
    }
}