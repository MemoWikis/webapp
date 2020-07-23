using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
 
    public static List<QuestionListJson.Question> PopulateQuestionsOnPage(int categoryId, int currentPage, int itemCount, bool isLoggedIn,int questionsSort = 2)
    {
        var allQuestions = GetAllQuestions(categoryId);
        var user = isLoggedIn ? Sl.R<SessionUser>().User : null;

        ConcurrentDictionary<int, QuestionValuation> userQuestionValuation = new ConcurrentDictionary<int, QuestionValuation>(); 
        if(user != null)
             userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;

        switch (questionsSort)
        {
            case (int)QuestionSort.Alphabetical:
                 allQuestions = allQuestions.OrderBy(q => q.Text).ToList();
                break;
            case (int)QuestionSort.Random:
            case (int)QuestionSort.Knowledge:
                break;
            case (int)QuestionSort.Probability:
                allQuestions = allQuestions.OrderBy(q => q.CorrectnessProbability).ToList();
                break;
        }

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

            if (user != null)
            {
                
                
                if (userQuestionValuation.ContainsKey(q.Id))
                {
                    question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                    question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge();
                    question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
                }
            }

            newQuestionList.Add(question);
        }

        //Logg.r().Error("QuestionListModel/PopulateQuestionsOnPage, unknown sorting function");
        return newQuestionList;
    }
}