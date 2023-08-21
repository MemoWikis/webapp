﻿using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using MySqlX.XDevAPI;

public class CategoryInKnowledge :IRegisterAsInstancePerLifetime
{
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly SessionUserCache _sessionUserCache;

    public CategoryInKnowledge(QuestionInKnowledge questionInKnowledge,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        SessionUserCache sessionUserCache)
    {
        _questionInKnowledge = questionInKnowledge;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _sessionUserCache = sessionUserCache;
    }

    private IList<int> QuestionsInValuatedCategories(int userId, IList<int> questionIds, int exeptCategoryId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var valuatedCategories = _sessionUserCache.GetCategoryValuations(userId).Where(v => v.IsInWishKnowledge());

        if (exeptCategoryId != -1)
            valuatedCategories = valuatedCategories.Where(v => v.CategoryId != exeptCategoryId);


        var questionsInOtherValuatedCategories = valuatedCategories
            .SelectMany(v =>
            {
                var category =EntityCache.GetCategory(v.CategoryId);

                return category == null ? 
                    new List<QuestionCacheItem>() : 
                    category.GetAggregatedQuestionsFromMemoryCache(userId);
            })
            .GetIds()
            .Distinct()
            .ToList();

        return questionsInOtherValuatedCategories;
    }

    public void UnpinQuestionsInCategoryInDatabase(int categoryId, int userId, SessionUser sessionUser)
    {
        var user = _userReadingRepo.GetByIds(userId).First();
        var questionsInCategory = EntityCache.GetCategory(categoryId).GetAggregatedQuestionsFromMemoryCache(userId);
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedCategories = QuestionsInValuatedCategories(user.Id, questionIds, exeptCategoryId: categoryId);

        var questionInOtherPinnedEntitites = questionsInPinnedCategories;
        var questionsToUnpin = questionsInCategory.Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id)).ToList();

        foreach (var question in questionsToUnpin)
            _questionInKnowledge.Unpin(question.Id, user.Id);

        _questionInKnowledge.UpdateTotalRelevancePersonalInCache(questionsToUnpin);
        _questionInKnowledge.SetUserWishCountQuestions(user.Id, sessionUser);
    }
}