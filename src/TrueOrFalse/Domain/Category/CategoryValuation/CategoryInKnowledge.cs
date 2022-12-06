using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using FluentNHibernate.Conventions;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;

public class CategoryInKnowledge
{
    private static void UpdateProbabilityForQuestionValuation(QuestionCacheItem question, UserCacheItem user, IList<Answer> answers, QuestionValuationCacheItem userQuestionValuation)
    {
        var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(question, user, answers);

        userQuestionValuation.CorrectnessProbability = probabilityResult.Probability;
        userQuestionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
        userQuestionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
    }

    public static void UnpinQuestionsInCategory(int categoryId, UserCacheItem user)
    {
        if (user.Id == -1) { throw new Exception("user not existent"); }

        CreateJob(JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge,
            new CategoryUserPair { CategoryId = categoryId, UserId = user.Id });

        var questionsInCategory = EntityCache.GetCategory(categoryId).GetAggregatedQuestionsFromMemoryCache();
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedCategories = QuestionsInValuatedCategories(user.Id, questionIds, categoryId);
        var questionInOtherPinnedEntitites = questionsInPinnedCategories;
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id)).ToList();

        var questionValuations = SessionUserCache.GetItem(user.Id).QuestionValuations;
        foreach (var question in questionsToUnpin)
        {
            var questionValuation = questionValuations.FirstOrDefault(v => v.Value.Question.Id == question.Id).Value;
            CreateOrUpdateQuestionValution(question, user, false, questionValuation);
        }

        QuestionInKnowledge.SetUserWishCountQuestions(user.Id);
    }

    private static void CreateOrUpdateQuestionValution(
        QuestionCacheItem question, 
        UserCacheItem user,
        bool isInWishKnowledge,
        QuestionValuationCacheItem userQuestionValuation,
        IList<Answer> answersForProbabilityUpdate = null)
    {
        if (userQuestionValuation == null)
        {
            userQuestionValuation = new QuestionValuationCacheItem()
            {
                Question = question,
                User = user,
                IsInWishKnowledge = isInWishKnowledge
            };
        }
        else
        {
            userQuestionValuation.IsInWishKnowledge = isInWishKnowledge;
        }

        if(isInWishKnowledge && answersForProbabilityUpdate != null)
            UpdateProbabilityForQuestionValuation(question, user, answersForProbabilityUpdate, userQuestionValuation);

        Sl.QuestionValuationRepo.CreateOrUpdateInCache(userQuestionValuation);
    }

    private static void CreateJob(JobQueueType jobType, CategoryUserPair jobContent)
    {
        var serializer = new JavaScriptSerializer();
        var categoryUserPairJsonString =
            serializer.Serialize(jobContent);
        Sl.R<JobQueueRepo>().Add(jobType, categoryUserPairJsonString);
    }

    private static IList<int> QuestionsInValuatedCategories(int userId, IList<int> questionIds, int exeptCategoryId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var valuatedCategories = SessionUserCache.GetCategoryValuations(userId).Where(v => v.IsInWishKnowledge());

        if (exeptCategoryId != -1)
            valuatedCategories = valuatedCategories.Where(v => v.CategoryId != exeptCategoryId);


        var questionsInOtherValuatedCategories = valuatedCategories
            .SelectMany(v =>
            {
                var category =EntityCache.GetCategory(v.CategoryId);

                return category == null ? 
                    new List<QuestionCacheItem>() : 
                    category.GetAggregatedQuestionsFromMemoryCache();
            })
            .GetIds()
            .Distinct()
            .ToList();

        return questionsInOtherValuatedCategories;
    }

    public static void UnpinQuestionsInCategoryInDatabase(int categoryId, int userId)
    {
        var user = Sl.UserRepo.GetByIds(userId).First();
        var questionsInCategory = EntityCache.GetCategory(categoryId).GetAggregatedQuestionsFromMemoryCache();
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedCategories = QuestionsInValuatedCategories(user.Id, questionIds, exeptCategoryId: categoryId);

        var questionInOtherPinnedEntitites = questionsInPinnedCategories;
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id)).ToList();

        foreach (var question in questionsToUnpin)
            QuestionInKnowledge.Unpin(question.Id, user.Id);

        QuestionInKnowledge.UpdateTotalRelevancePersonalInCache(questionsToUnpin);
        QuestionInKnowledge.SetUserWishCountQuestions(user.Id);
    }

    private static void PinQuestionsInCategory(int categoryId, User user)
    {
        var category = EntityCache.GetCategory(categoryId);
        if (category == null) return;
        var questions = category.GetAggregatedQuestionsFromMemoryCache();
        QuestionInKnowledge.Pin(questions, user);
    }
}