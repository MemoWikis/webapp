﻿using System.Diagnostics;

public class EntityCacheInitializer(
    CategoryRepository _categoryRepository,
    UserReadingRepo _userReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    CategoryRelationRepo _categoryRelationRepo,
    CategoryViewRepo _categoryViewRepo) : IRegisterAsInstancePerLifetime
{
    public void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r.Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allUsers = _userReadingRepo.GetAll();
        Logg.r.Information("EntityCache UsersLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var users = UserCacheItem.ToCacheUsers(allUsers).ToList();
        Logg.r.Information("EntityCache UsersCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        Cache.IntoForeverCache(EntityCache.CacheKeyUsers, users.ToConcurrentDictionary());

        var allCategories = _categoryRepository.GetAllEager();
        Logg.r.Information("EntityCache CategoriesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var allRelations = _categoryRelationRepo.GetAll();
        Logg.r.Information("EntityCache CategoryRelationsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var relations = CategoryCacheRelation.ToCategoryCacheRelations(allRelations).ToList();
        Cache.IntoForeverCache(EntityCache.CacheKeyRelations, relations.ToConcurrentDictionary());
        Logg.r.Information("EntityCache CategoryRelationsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
       

        var categories = CategoryCacheItem.ToCacheCategories(allCategories ).ToList();
        Logg.r.Information("EntityCache CategoriesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Cache.IntoForeverCache(EntityCache.CacheKeyCategories, categories.ToConcurrentDictionary());


        var categoriesViewsLast30Days = _categoryViewRepo.GetViewsForLastNDaysGroupByCategoryId(30);
        foreach (var categoryCacheItem in categories)
        {
            var aggregatedCategories = categoryCacheItem.AggregatedCategoriesForEntityCacheInitilizer()
                .Select(t => t.Key);

            var aggregatedViews = categoriesViewsLast30Days
                .Where(view => aggregatedCategories.Contains(view.Category_Id))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new TopicView { Date = v.Date, Views = v.TotalCount })
                .ToList();

            categoryCacheItem.AddViews(aggregatedViews);

        }

        Logg.r.Information("EntityCache CategoriesPutIntoForeverCache " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var allQuestions = _questionReadingRepo.GetAllEager();
        Logg.r.Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions).ToList();
        Logg.r.Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Logg.r.Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        Cache.IntoForeverCache(EntityCache.CacheKeyQuestions, questions.ToConcurrentDictionary());

        Cache.IntoForeverCache(EntityCache.CacheKeyCategoryQuestionsList,
            EntityCache.GetCategoryQuestionsListForCacheInitilizer(questions));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            EntityCache.Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }
        Logg.r.Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        EntityCache.IsFirstStart = false;
    }
}

