using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using FluentNHibernate.Conventions;
using NHibernate;
using TrueOrFalse.Utilities.ScheduledJobs;

public class CategoryInKnowledge
{
    public static void Pin(int categoryId, User user)
    {
        var serializer = new JavaScriptSerializer();
        var categoryUserPairJsonString =
            serializer.Serialize(new CategoryUserPair {CategoryId = categoryId, UserId = user.Id});
        Sl.R<JobQueueRepo>().Add(JobQueueType.AddCategoryToWishKnowledge, categoryUserPairJsonString);

        var questions = Sl.CategoryRepo.GetById(categoryId).GetAggregatedQuestionsFromMemoryCache();
        var userQuestionValuation = UserValuationCache.GetItem(user.Id).QuestionValuations;
        var userCategoryAnswers = Sl.R<AnswerRepo>().GetByUserAndCategory(user.Id, categoryId);
        foreach (var question in questions)
        {
            if (user.Id == -1)
                return;

            var questionValuation =
                userQuestionValuation.FirstOrDefault(x => x.Value.Question.Id == question.Id).Value ??
                new QuestionValuation
                {
                    Question = question,
                    User = user
                };



            //var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(question, user); //Performance 
            //probability calc start
            ProbabilityCalcResult probabilityResult;
            if (Sl.Session.Get<Question>(question.Id) == null || Sl.Session.Get<User>(user.Id) == null)
                probabilityResult = new ProbabilityCalcResult { Probability = 0, KnowledgeStatus = KnowledgeStatus.NotLearned };
            else
                probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(userCategoryAnswers.Where(a => a.Question == question).ToList(), question, user);

            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
            //probability calc stop

            UserValuationCache.AddOrUpdate(questionValuation);
        }
        ////PinQuestionsInCategory(categoryId, user);
        ////UpdateCategoryValuation(categoryId, user, 50);
    }

    public static void PinQuestionsInCategory(int categoryId, User user)
    {
        var questions = Sl.CategoryRepo.GetById(categoryId).GetAggregatedQuestionsFromMemoryCache();
        QuestionInKnowledge.Pin(questions, user);
    }

    public static void Unpin(int categoryId, User user) => UpdateCategoryValuation(categoryId, user, -1);

    public static void UnpinQuestionsInCategory(int categoryId, User user)
    {
        var questionsInCategory = Sl.CategoryRepo.GetById(categoryId).GetAggregatedQuestionsFromMemoryCache();
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedSets = SetInKnowledge.ValuatedQuestionsInSets(user, questionIds);
        var questionsInPinnedCategories = QuestionsInValuatedCategories(user, questionIds, exeptCategoryId:categoryId);

        var questionInOtherPinnedEntitites = questionsInPinnedSets.Union(questionsInPinnedCategories);
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id));

        foreach (var question in questionsToUnpin)
            QuestionInKnowledge.Unpin(question.Id, user);
    }

    private static IList<int> QuestionsInValuatedCategories(User user, IList<int> questionIds, int exeptCategoryId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var valuatedCategories = UserValuationCache.GetCategoryValuations(user.Id).Where(v => v.IsInWishKnowledge());

        if (exeptCategoryId != -1)
            valuatedCategories = valuatedCategories.Where(v => v.CategoryId != exeptCategoryId);

        var catRepo = Sl.CategoryRepo;

        var questionsInOtherValuatedCategories = valuatedCategories
            .SelectMany(v => catRepo.GetById(v.CategoryId).GetAggregatedQuestionsFromMemoryCache())
            .GetIds()
            .Distinct()
            .ToList();

        return questionsInOtherValuatedCategories;
    }

    public static void UpdateCategoryValuation(int categoryId, User user, int relevance = 50)
    {
        CreateOrUpdateCategoryValuation.Run(categoryId, user.Id, relevancePeronal: relevance);

        var session = Sl.R<ISession>();
        session.CreateSQLQuery(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", categoryId)).ExecuteUpdate();
        session.Flush();

        ReputationUpdate.ForCategory(categoryId);
    }

    private static string GenerateEntriesQuery(string fieldToSet, string fieldSource, int categoryId)
    {
        return "UPDATE Category SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM CategoryValuation " +
                    "WHERE CategoryId = " + categoryId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + categoryId + ";";
    }
}