﻿using System;
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
    public static void Pin(int categoryId, User user)
    {
        if(user.Id == -1) { throw new Exception("user not existent");}

        CreateJob(JobQueueType.AddCategoryToWishKnowledge,
            new CategoryUserPair { CategoryId = categoryId, UserId = user.Id });

        var questions = EntityCache.GetCategoryCacheItem(categoryId, getDataFromEntityCache: true).GetAggregatedQuestionsFromMemoryCache();
        var questionValuations = UserCache.GetItem(user.Id).QuestionValuations;
        var userCategoryAnswers = Sl.R<AnswerRepo>().GetByQuestion(questions.GetIds().ToList(), user.Id);
        foreach (var question in questions)
        {
            var questionValuation = questionValuations.FirstOrDefault(v => v.Value.Question.Id == question.Id).Value;
            CreateOrUpdateQuestionValution(question, user, isInWishKnowledge:true, questionValuation, userCategoryAnswers);
        }
        QuestionInKnowledge.SetUserWishCountQuestions(user);
        UpdateCategoryValuation(categoryId, user);
    }

    private static void UpdateProbabilityForQuestionValuation(Question question, User user, IList<Answer> answers, QuestionValuationCacheItem userQuestionValuation)
    {
        var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(question, user, answers);

        userQuestionValuation.CorrectnessProbability = probabilityResult.Probability;
        userQuestionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
        userQuestionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
    }

    public static void Unpin(int categoryId, User user) => UpdateCategoryValuation(categoryId, user, -1);

    public static void UnpinQuestionsInCategory(int categoryId, User user)
    {
        if (user.Id == -1) { throw new Exception("user not existent"); }

        CreateJob(JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge,
            new CategoryUserPair { CategoryId = categoryId, UserId = user.Id });

        var questionsInCategory = EntityCache.GetCategoryCacheItem(categoryId).GetAggregatedQuestionsFromMemoryCache();
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedCategories = QuestionsInValuatedCategories(user, questionIds, categoryId);
        var questionInOtherPinnedEntitites = questionsInPinnedCategories;
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id)).ToList();

        var questionValuations = UserCache.GetItem(user.Id).QuestionValuations;
        foreach (var question in questionsToUnpin)
        {
            var questionValuation = questionValuations.FirstOrDefault(v => v.Value.Question.Id == question.Id).Value;
            CreateOrUpdateQuestionValution(question, user, false, questionValuation);
        }

        QuestionInKnowledge.SetUserWishCountQuestions(user);
    }

    private static void CreateOrUpdateQuestionValution(
        Question question, 
        User user,
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

    private static IList<int> QuestionsInValuatedCategories(User user, IList<int> questionIds, int exeptCategoryId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var valuatedCategories = UserCache.GetCategoryValuations(user.Id).Where(v => v.IsInWishKnowledge());

        if (exeptCategoryId != -1)
            valuatedCategories = valuatedCategories.Where(v => v.CategoryId != exeptCategoryId);


        var questionsInOtherValuatedCategories = valuatedCategories
            .SelectMany(v =>
            {
                var category =EntityCache.GetCategoryCacheItem(v.CategoryId);

                return category == null ? 
                    new List<Question>() : 
                    category.GetAggregatedQuestionsFromMemoryCache();
            })
            .GetIds()
            .Distinct()
            .ToList();

        return questionsInOtherValuatedCategories;
    }

    public static void PinCategoryInDatabase(int categoryId, int userId)
    {
        var user = Sl.UserRepo.GetById(userId);
        PinQuestionsInCategory(categoryId, user, SaveType.DatabaseOnly);
    }

    public static void UnpinQuestionsInCategoryInDatabase(int categoryId, int userId)
    {
        var user = Sl.UserRepo.GetByIds(userId).First();
        var questionsInCategory = EntityCache.GetCategoryCacheItem(categoryId).GetAggregatedQuestionsFromMemoryCache();
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedCategories = QuestionsInValuatedCategories(user, questionIds, exeptCategoryId: categoryId);

        var questionInOtherPinnedEntitites = questionsInPinnedCategories;
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id)).ToList();

        foreach (var question in questionsToUnpin)
            QuestionInKnowledge.Unpin(question.Id, user, SaveType.DatabaseOnly);

        QuestionInKnowledge.UpdateTotalRelevancePersonalInCache(questionsToUnpin);
        QuestionInKnowledge.SetUserWishCountQuestions(user);
    }

    private static void PinQuestionsInCategory(int categoryId, User user, SaveType saveType = SaveType.CacheAndDatabase)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);
        if (category == null) return;
        var questions = category.GetAggregatedQuestionsFromMemoryCache();
        QuestionInKnowledge.Pin(questions, user, saveType);
    }

    public static void UpdateCategoryValuationTest(int categoryId, User user, int relevance = 50)
    {
        UpdateCategoryValuation(categoryId, user, relevance);
    }
    private static void UpdateCategoryValuation(int categoryId, User user, int relevance = 50)
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