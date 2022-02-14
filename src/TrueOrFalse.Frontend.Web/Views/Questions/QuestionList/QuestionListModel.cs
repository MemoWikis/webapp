using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel : BaseModel
{
    public int CategoryId;
    public int ItemCount;
    public int AllQuestionsInCategory;
    public bool IsSessionNoteFadeIn { get; set; }

    public EditQuestionModel EditQuestionModel;


    public QuestionListModel(int categoryId, bool isSessionNoteFadeIn = true)
    {
        CategoryId = categoryId;
        AllQuestionsInCategory = Sl.CategoryRepo.CountAggregatedQuestions(categoryId);
        IsSessionNoteFadeIn = isSessionNoteFadeIn;

        var editQuestionModel = new EditQuestionModel();
        editQuestionModel.Categories.Add(Sl.CategoryRepo.GetByIdEager((int)categoryId));

        EditQuestionModel = editQuestionModel;
    }

    public static List<QuestionListJson.Question> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage)
    {
        var userQuestionValuation = Sl.SessionUser.IsLoggedIn ? UserCache.GetItem(Sl.SessionUser.UserId).QuestionValuations : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        var learningSession = LearningSessionCache.GetLearningSession();
        var steps = learningSession.Steps;
        var stepsOfCurrentPage = steps.Skip(itemCountPerPage * (currentPage - 1)).Take(itemCountPerPage);

        var newQuestionList = new List<QuestionListJson.Question>();

        foreach (var step in stepsOfCurrentPage)
        {
            var q = step.Question;
            var question = new QuestionListJson.Question
            {
                Id = q.Id,
                Title = q.Text,
                LinkToQuestion = Links.GetUrl(q),
                ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url,
                LearningSessionStepCount = steps.Count,
                LinkToEditQuestion = Links.EditQuestion(q.Text, q.Id),
                LinkToQuestionVersions = Links.QuestionHistory(q.Id),
                LinkToComment = Links.GetUrl(q) + "#JumpLabel",
                CorrectnessProbability = q.CorrectnessProbability,
                Visibility = q.Visibility,
                SessionIndex = steps.IndexOf(step),
            };

            if (userQuestionValuation.ContainsKey(q.Id) && Sl.SessionUser.IsLoggedIn)
            {
                question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
                question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }
            newQuestionList.Add(question);
        }

        return newQuestionList;
    }
}