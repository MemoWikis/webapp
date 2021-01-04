using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel : BaseModel
{
    public int CategoryId;
    public int ItemCount;
    public int AllQuestionsInCategory;
    public bool IsSessionNoteFadeIn { get; set; }


    public QuestionListModel(int categoryId, bool isSessionNoteFadeIn = true)
    {
        CategoryId = categoryId;
        AllQuestionsInCategory = Sl.CategoryRepo.CountAggregatedQuestions(categoryId);
        IsSessionNoteFadeIn = isSessionNoteFadeIn;
    }

    public static List<QuestionListJson.Question> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage, bool isLoggedIn, int categoryId)
    {
        IEnumerable<Question> allQuestions;
        var user = isLoggedIn ? Sl.R<SessionUser>().User : null;

        ConcurrentDictionary<int, QuestionValuation> userQuestionValuation = new ConcurrentDictionary<int, QuestionValuation>();
        if (user != null)
        {
            allQuestions = Sl.SessionUser.LearningSession.Steps.Select(q => q.Question);
            userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;
        }
        else
            allQuestions = GetAllQuestions(categoryId);

        var questionsOfCurrentPage = allQuestions.Skip(itemCountPerPage * (currentPage - 1)).Take(itemCountPerPage).ToList();
        var newQuestionList = new List<QuestionListJson.Question>();
        var learningSessionStepCount = allQuestions.Count();

        foreach (var q in questionsOfCurrentPage)
        {
            var question = new QuestionListJson.Question();
            question.Id = q.Id;
            question.Title = Regex.Replace(q.Text, "<.*?>", String.Empty);
            question.LinkToQuestion = Links.GetUrl(q);
            question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
            question.LearningSessionStepCount = learningSessionStepCount;
            question.LinkToQuestion = Links.GetUrl(q);
            question.LinkToEditQuestion = Links.EditQuestion( q.Text, q.Id);
            question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
            question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
            question.CorrectnessProbability = q.CorrectnessProbability;
            if (Sl.SessionUser.LearningSession != null)
            {
                var steps = Sl.SessionUser.LearningSession.Steps;
                var index = steps.IndexOf(s => s.Question.Id == q.Id);
                question.SessionIndex = index;
            }

            if (userQuestionValuation.ContainsKey(q.Id) && user != null)
            {
                question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge();
                question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }
            newQuestionList.Add(question);
        }
        return newQuestionList;
    }
    private static List<Question> GetAllQuestions(int categoryId)
    {
        return EntityCache.GetQuestionsForCategory(categoryId).ToList();
    }
}