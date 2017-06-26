using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;

public class EntityCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeyCategoryRelations = "allCategoryRelations_EntityCache";
    private const string _cacheKeySets = "allSets_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";

    public static Dictionary<int, Question> Questions => (Dictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    public static Dictionary<int, Category> Categories => (Dictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];
    public static Dictionary<int, CategoryRelation> CategoryRelations => (Dictionary<int, CategoryRelation>)HttpRuntime.Cache[_cacheKeyCategoryRelations];
    public static Dictionary<int, Set> Sets => (Dictionary<int, Set>)HttpRuntime.Cache[_cacheKeySets];

    public static Dictionary<int, IList<Question>> CategoryQuestionsList => (Dictionary<int, IList<Question>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];
     
    public static void Init()
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start {Elapsed}", stopWatch.Elapsed);
        
        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAll();
        var aggregatedCategoryRelations = Sl.CategoryRelationRepo.GetAll();
        var sets = Sl.SetRepo.GetAllEager();

        Logg.r().Information("EntityCache LoadAllEntities {Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToDictionary(q => q.Id, q => q));
        IntoForeverCache(_cacheKeyCategories, categories.ToDictionary(c => c.Id, c => c));
        IntoForeverCache(_cacheKeyCategoryRelations, aggregatedCategoryRelations.ToDictionary(r => r.Id, r => r));
        IntoForeverCache(_cacheKeySets, sets.ToDictionary(s => s.Id, s => s));
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions));

        Logg.r().Information("EntityCache PutIntoCache {Elapsed}", stopWatch.Elapsed);
    }

    private static Dictionary<int, IList<Question>> GetCategoryQuestionsList(IList<Question> questions)
    {
        var categoryQuestionList = new Dictionary<int, IList<Question>>();
        foreach (var question in questions)
        {
            foreach (var category in question.Categories)
            {
                if (!categoryQuestionList.ContainsKey(category.Id))
                    categoryQuestionList.Add(category.Id, new List<Question>());

                categoryQuestionList[category.Id].Add(question);
            }
        }

        return categoryQuestionList;
    }

    private static void IntoForeverCache<T>(string key, Dictionary<int, T> objectToCache)
    {
        HttpRuntime.Cache.Insert(
            key, 
            objectToCache, 
            null, 
            Cache.NoAbsoluteExpiration,
            Cache.NoSlidingExpiration,
            CacheItemPriority.NotRemovable, 
            null);
    }
}