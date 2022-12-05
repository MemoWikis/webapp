using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Search;

public class CategoryRepository : RepositoryDbBase<Category>
{
    private readonly SearchIndexCategory _searchIndexCategory;

    public CategoryRepository(ISession session, SearchIndexCategory searchIndexCategory)
        : base(session)
    {
        _searchIndexCategory = searchIndexCategory;
        _searchIndexCategory = searchIndexCategory;
    }

    public Category GetByIdEager(int categoryId) => GetByIdsEager(new[] { categoryId }).FirstOrDefault();
    public Category GetByIdEager(CategoryCacheItem category) => GetByIdsEager(new[] { category.Id }).FirstOrDefault();

    public IList<Category> GetByIdsEager(IEnumerable<int> categoryIds = null)
    {
        var query = _session.QueryOver<Category>();
        if (categoryIds != null)
            query = query.Where(Restrictions.In("Id", categoryIds.ToArray()));
        else
        {
            //warmup entity cache
            var users = _session
                .QueryOver<User>()
                .Fetch(SelectMode.Fetch, u => u.MembershipPeriods)
                .List();
        }

        var result = query.Left.JoinQueryOver<CategoryRelation>(s => s.CategoryRelations)
            .Left.JoinQueryOver(x => x.RelatedCategory)
            .List()
            .GroupBy(c => c.Id)
            .Select(c => c.First())
            .ToList();

        foreach (var category in result)
        {
            NHibernateUtil.Initialize(category.Creator);
            NHibernateUtil.Initialize(category.CategoryRelations);
        }
        return result;
    }

    public IList<Category> GetAllEager() => GetByIdsEager();

    /// <summary>
    /// Add Category in Database,
    /// </summary>
    /// <param name="category"></param>
    public override void Create(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default(DateTime)))
            related.DateModified = related.DateCreated = DateTime.Now;

        base.Create(category);
        Flush();
       
        UserActivityAdd.CreatedCategory(category);
        _searchIndexCategory.Update(category);

        var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(categoryCacheItem);

        Sl.CategoryChangeRepo.AddCreateEntry(category, category.Creator.Id);

        GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(categoryCacheItem);
        UpdateCachedData(categoryCacheItem, CreateDeleteUpdate.Create);

        if (category.ParentCategories().Count != 1)
            Logg.r().Warning("the parentcounter is != 1");

        var parentCategories = category.ParentCategories();

        if (parentCategories.Count != 0)
        {
            Sl.CategoryChangeRepo.AddUpdateEntry(category, SessionUser.User.UserId, false, type: CategoryChangeType.Relations);
        }
    }

    public void CreateOnlyDb(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default(DateTime)))
            related.DateModified = related.DateCreated = DateTime.UtcNow.AddHours(2);
        base.Create(category);
        Flush();
        _searchIndexCategory.Update(category);
        Sl.CategoryChangeRepo.AddCreateEntryDbOnly(category, category.Creator);
    }

    public static void UpdateCachedData(CategoryCacheItem categoryCacheItem, CreateDeleteUpdate createDeleteUpdate)
    {
        //Create
        if (createDeleteUpdate == CreateDeleteUpdate.Create)
        {
            //Update EntityCache
            var parents = EntityCache.GetCategories(GraphService.GetDirectParentIds(categoryCacheItem));
            foreach (var parent in parents)
            {
                parent.CachedData.AddChildId(categoryCacheItem.Id);
            }
        }

        //Update
        if (createDeleteUpdate == CreateDeleteUpdate.Update)
        {
            var oldCategoryCacheItem1 = EntityCache.GetCategory(categoryCacheItem.Id);

            var parentIdsCacheItem1 = categoryCacheItem.CategoryRelations
                .Select(cr => cr.RelatedCategoryId).ToList();

            var parentIdsOldCategoryCacheItem1 = oldCategoryCacheItem1.CategoryRelations
                .Select(cr => cr.RelatedCategoryId).ToList();

            var exceptIdsToDelete1 = parentIdsOldCategoryCacheItem1.Except(parentIdsCacheItem1).ToList();
            var exceptIdsToAdd1 = parentIdsCacheItem1.Except(parentIdsOldCategoryCacheItem1).ToList();

            if (exceptIdsToAdd1.Any() || exceptIdsToDelete1.Any())
            {
                foreach (var id in exceptIdsToAdd1)
                    EntityCache.GetCategory(id).CachedData
                        .AddChildId(categoryCacheItem.Id);

                foreach (var id in exceptIdsToDelete1)
                    EntityCache.GetCategory(id).CachedData.RemoveChildId(categoryCacheItem.Id);
            }
        }

        //Delete
        if (createDeleteUpdate == CreateDeleteUpdate.Delete)
        {
            //EntityCache
            foreach (var parent in categoryCacheItem.ParentCategories(true))
            {
                parent.CachedData.RemoveChildId(categoryCacheItem.Id);
            }
        }
    }

    private static IEnumerable<int> GetIdsToRemove(CategoryCacheItem oldCategoryCacheItem)
    {
        return oldCategoryCacheItem.CategoryRelations
            .Select(cr => cr.RelatedCategoryId);
    }

    public enum CreateDeleteUpdate
    {
        Create = 1,
        Delete = 2,
        Update = 3
    }

    public void UpdateAuthors(Category category)
    {
        var sql = "UPDATE category " +
                  "SET AuthorIds = " +"'" + category.AuthorIds + "'" +
                  " WHERE Id = " + category.Id;
        _session.CreateSQLQuery(sql).ExecuteUpdate();
    }

    /// <summary>
    /// Update method for internal purpose, takes care that no change sets are created.
    /// </summary>
    public override void Update(Category category) => Update(category);

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void Update(Category category, SessionUserCacheItem author = null, bool imageWasUpdated = false, bool isFromModifiyRelations = false, CategoryChangeType type = CategoryChangeType.Update, bool createCategoryChange = true, int[] affectedParentIdsByMove = null)
    {
        if (!isFromModifiyRelations)
            _searchIndexCategory.Update(category);

        base.Update(category);

        if (author != null && createCategoryChange)
            Sl.CategoryChangeRepo.AddUpdateEntry(category, author.UserId, imageWasUpdated, type, affectedParentIdsByMove);

        Flush();
        Sl.R<UpdateQuestionCountForCategory>().Run(category);
    }

    public void UpdateWithoutCaches(Category category, User author = null, bool imageWasUpdated = false,
        bool isFromModifiyRelations = false)
    {
        if (!isFromModifiyRelations)
            _searchIndexCategory.Update(category);

        base.Update(category);

        if (author != null)
            Sl.CategoryChangeRepo.AddUpdateEntry(category, author.Id, imageWasUpdated);

        Flush();

        Sl.R<UpdateQuestionCountForCategory>().Run(category);
    }




    public void UpdateBeforeEntityCacheInit(Category category)
    {
        _searchIndexCategory.Update(category);
        base.Update(category);

        Flush();
    }

    public override void Delete(Category category)
    {
        _searchIndexCategory.Delete(category);
        base.Delete(category);
        EntityCache.Remove(EntityCache.GetCategory(category));
    }

    public override void DeleteWithoutFlush(Category category)
    {
        _searchIndexCategory.Delete(category);
        base.DeleteWithoutFlush(category);
        EntityCache.Remove(EntityCache.GetCategory(category.Id));
        SessionUserCache.RemoveAllForCategory(category.Id);
    }

    public IList<Category> GetByName(string categoryName)
    {
        categoryName = categoryName ?? "";

        return _session.CreateQuery("from Category as c where c.Name = :categoryName")
                        .SetString("categoryName", categoryName)
                        .List<Category>();
    }

    public IList<Category> GetByIds(List<int> questionIds)
    {
        return GetByIds(questionIds.ToArray());
    }

    public IList<Category> GetByIdsFromString(string idsString)
    {
        return idsString
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x))
                .Select(GetById)
                .Where(set => set != null)
                .ToList();
    }

    public IList<Category> GetIncludingCategories(Category category, bool includingSelf = true)
    {
        var includingCategories = GetCategoriesIdsForRelatedCategory(category);

        if (includingSelf)
            includingCategories = includingCategories.Union(new List<Category> { category }).ToList();

        return includingCategories;
    }

    public IList<Category> GetCategoriesIdsForRelatedCategory(Category relatedCategory)
    {
        var query = _session.QueryOver<CategoryRelation>()
            .Where(r => r.RelatedCategory == relatedCategory);

        return query.List()
            .Select(r => r.Category)
            .ToList();
    }


    public IList<Category> GetChildren(int categoryId)
    {
        var categoryIds = _session.CreateSQLQuery($@"SELECT Category_id
            FROM relatedcategoriestorelatedcategories
            WHERE  Related_id = {categoryId}").List<int>();
        return GetByIds(categoryIds.ToArray());
    }

    public IList<Category> GetChildren(
        CategoryType parentType,
        CategoryType childrenType,
        int parentId,
        string searchTerm = "")
    {
        Category relatedCategoryAlias = null;
        Category categoryAlias = null;

        var query = Session
            .QueryOver<CategoryRelation>()
            .JoinAlias(c => c.RelatedCategory, () => relatedCategoryAlias)
            .JoinAlias(c => c.Category, () => categoryAlias)
            .Where(r => relatedCategoryAlias.Type == parentType
                        && relatedCategoryAlias.Id == parentId
                        && categoryAlias.Type == childrenType);

        if (!String.IsNullOrEmpty(searchTerm))
            query.WhereRestrictionOn(r => categoryAlias.Name)
                .IsLike(searchTerm);

        return query.Select(r => r.Category).List<Category>();
    }

    public IList<Category> GetDescendants(int parentId)
    {
        var currentGeneration = EntityCache
            .GetCategory(parentId)
            .CachedData.ChildrenIds
            .Select(id => Sl.CategoryRepo.GetByIdEager(id))
            .ToList();

        var nextGeneration = new List<Category>();
        var descendants = new List<Category>();

        while (currentGeneration.Count > 0)
        {
            descendants.AddRange(currentGeneration);

            foreach (var category in currentGeneration)
            {
                var children = EntityCache.GetCategory(category.Id)
                    .CachedData
                    .ChildrenIds
                    .Select(id => Sl.CategoryRepo.GetByIdEager(id))
                    .ToList();

                if (children.Count > 0)
                {
                    nextGeneration.AddRange(children);
                }
            }

            currentGeneration = nextGeneration.Except(descendants).Where(c => c.Id != parentId).Distinct().ToList();
            nextGeneration = new List<Category>();
        }

        return descendants;
    }

    public int CountAggregatedQuestions(int categoryId)
    {
        var count = _session.CreateSQLQuery($@"
        SELECT COUNT(DISTINCT(questionId)) FROM 
        (
	        SELECT DISTINCT(cq.Question_id) questionId
            FROM categories_to_questions cq
            WHERE cq.Category_id = {categoryId}

	        UNION
	
	        SELECT DISTINCT(cq.Question_id) questionId 
	        FROM relatedcategoriestorelatedcategories rc
	        INNER JOIN category c
	        ON rc.Related_Id = c.Id
	        INNER JOIN categories_to_questions cq
	        ON c.Id = cq.Category_id
	        WHERE rc.Category_id = {categoryId}
        ) c
        ").UniqueResult<long>();//Union is distinct by default
        return (int)count;
    }

    public override IList<Category> GetByIds(params int[] categoryIds)
    {
        var resultTmp = base.GetByIds(categoryIds);

        var result = new List<Category>();
        for (int i = 0; i < categoryIds.Length; i++)
        {
            if (resultTmp.Any(c => c.Id == categoryIds[i]))
                result.Add(resultTmp.First(c => c.Id == categoryIds[i]));
        }
        return result;
    }

    public IEnumerable<Category> GetWithMostQuestions(int amount)
    {
        return _session
            .QueryOver<Category>()
            .OrderBy(c => c.CountQuestionsAggregated).Desc
            .Take(amount)
            .List();
    }

    public IEnumerable<Category> GetMostRecent_WithAtLeast3Questions(int amount)
    {
        return _session
            .QueryOver<Category>()
            .Where(c => c.CountQuestionsAggregated > 3 || c.CountQuestions > 3)
            .OrderBy(c => c.DateCreated)
            .Desc
            .Take(amount)
            .List();
    }

    public bool Exists(string categoryName)
    {
        return GetByName(categoryName).Any(x => x.Type == CategoryType.Standard);
    }


    public int TotalCategoryCount()
    {
        return _session.QueryOver<Category>()
            .RowCount();
    }

    public const int AllgemeinwissenId = 709;
    public const int StudiumId = 687;
    public const int SchuleId = 682;
    public const int ZertifikateId = 689;

    public IEnumerable<int> GetRootCategoryInts() => GetRootCategoryListIds();

    public List<int> GetRootCategoryListIds()
    {
        return new List<int>
        {
            SchuleId,
            StudiumId,
            ZertifikateId,
            AllgemeinwissenId
        };
    }
}