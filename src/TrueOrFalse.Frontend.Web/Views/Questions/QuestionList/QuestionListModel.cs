using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel : BaseModel
{
    public int CategoryId;
    public int ItemCount;
    public int AllQuestionsInCategory; 


    public QuestionListModel(int categoryId)
    {
        CategoryId = categoryId;
        AllQuestionsInCategory = Sl.CategoryRepo.CountAggregatedQuestions(categoryId); //32 ms Duration

    }

    public static List<QuestionListJson.Question> PopulateQuestionsOnPage(int currentPage, int itemCount, bool isLoggedIn)
    {
        var allQuestions = Sl.SessionUser.LearningSession.Steps.Select(q => q.Question);
        var user = isLoggedIn ? Sl.R<SessionUser>().User : null;

        ConcurrentDictionary<int, QuestionValuation> userQuestionValuation = new ConcurrentDictionary<int, QuestionValuation>(); 
        if(user != null)
             userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;

        var questionsOfCurrentPage = allQuestions.Skip(itemCount * (currentPage - 1)).Take(itemCount).ToList();
        var newQuestionList = new List<QuestionListJson.Question>();

        foreach (Question q in questionsOfCurrentPage)
        {
            var question = new QuestionListJson.Question();
            question.Id = q.Id;
            question.Title = q.Text;
            question.LinkToQuestion = Links.GetUrl(q);
            question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;

            question.CorrectnessProbability = q.CorrectnessProbability;

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
}