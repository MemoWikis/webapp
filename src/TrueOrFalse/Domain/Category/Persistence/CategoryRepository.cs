using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse.Search;

public class CategoryRepository(
    ISession session,
    CategoryChangeRepo categoryChangeRepo,
    UpdateQuestionCountForCategory updateQuestionCountForCategory,
    UserReadingRepo userReadingRepo,
    UserActivityRepo userActivityRepo)
    : RepositoryDbBase<Category>(session)
{
    /// <summary>
    ///     Add Category in Database,
    /// </summary>
    /// <param name="category"></param>
    public override void Create(Category category)
    {
        base.Create(category);
        Flush();

        UserActivityAdd.CreatedCategory(category, userReadingRepo, userActivityRepo);

        var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(categoryCacheItem);

        categoryChangeRepo.AddCreateEntry(this, category, category.Creator?.Id ?? -1);

        Task.Run(async () => await new MeiliSearchCategoriesDatabaseOperations()
            .CreateAsync(category)
            .ConfigureAwait(false));
    }

    public IList<Category> GetAllEager()
    {
        return GetByIdsEager();
    }

    public Category GetByIdEager(int categoryId)
    {
        return GetByIdsEager(new[] { categoryId }).FirstOrDefault();
    }

    public Category GetByIdEager(CategoryCacheItem category)
    {
        return GetByIdsEager(new[] { category.Id }).FirstOrDefault();
    }

    public IList<Category> GetByIds(List<int> questionIds)
    {
        return GetByIds(questionIds.ToArray());
    }

    public override IList<Category> GetByIds(params int[] categoryIds)
    {
        var resultTmp = base.GetByIds(categoryIds);

        var result = new List<Category>();
        for (var i = 0; i < categoryIds.Length; i++)
        {
            if (resultTmp.Any(c => c.Id == categoryIds[i]))
            {
                result.Add(resultTmp.First(c => c.Id == categoryIds[i]));
            }
        }

        return result;
    }

    public IList<Category> GetByIdsEager(IEnumerable<int> categoryIds = null)
    {
        var query = _session.QueryOver<Category>();
        if (categoryIds != null)
        {
            query = query.Where(Restrictions.In("Id", categoryIds.ToArray()));
        }

        var result = query
            .List()
            .GroupBy(c => c.Id)
            .Select(c => c.First())
            .ToList();

        foreach (var category in result)
        {
            NHibernateUtil.Initialize(category.Creator);
        }

        return result;
    }

    public IList<Category> GetByName(string categoryName)
    {
        categoryName ??= "";

        return _session.CreateQuery("from Category as c where c.Name = :categoryName")
            .SetString("categoryName", categoryName)
            .List<Category>();
    }

    public IList<Category> GetCategoriesIdsForRelatedCategory(Category relatedCategory)
    {
        var query = _session.QueryOver<CategoryRelation>()
            .Where(r => r.Parent == relatedCategory);

        return query.List()
            .Select(r => r.Child)
            .ToList();
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
            .JoinAlias(c => c.Parent, () => relatedCategoryAlias)
            .JoinAlias(c => c.Child, () => categoryAlias)
            .Where(r => relatedCategoryAlias.Type == parentType
                        && relatedCategoryAlias.Id == parentId
                        && categoryAlias.Type == childrenType);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.WhereRestrictionOn(r => categoryAlias.Name)
                .IsLike(searchTerm);
        }

        return query.Select(r => r.Child).List<Category>();
    }

    public IList<Category> GetIncludingCategories(Category category, bool includingSelf = true)
    {
        var includingCategories = GetCategoriesIdsForRelatedCategory(category);

        if (includingSelf)
        {
            includingCategories =
                includingCategories.Union(new List<Category> { category }).ToList();
        }

        return includingCategories;
    }

    /// <summary>
    ///     Update method for internal purpose, takes care that no change sets are created.
    /// </summary>
    public override void Update(Category category)
    {
        Update(category);
    }

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void Update(
        Category category,
        int authorId = 0,
        bool imageWasUpdated = false,
        bool isFromModifiyRelations = false,
        CategoryChangeType type = CategoryChangeType.Update,
        bool createCategoryChange = true,
        int[] affectedParentIdsByMove = null)
    {
        base.Update(category);

        if (authorId != 0 && createCategoryChange)
        {
            categoryChangeRepo.AddUpdateEntry(this, category, authorId, imageWasUpdated, type,
                affectedParentIdsByMove);
        }

        Flush();
        updateQuestionCountForCategory.RunForJob(category, authorId);
        Task.Run(async () =>
        {
            await new MeiliSearchCategoriesDatabaseOperations()
                .UpdateAsync(category)
                .ConfigureAwait(false);
        });
    }

    public void CreateOnlyDb(Category category)
    {
        base.Create(category);
        Flush();

        categoryChangeRepo.AddCreateEntryDbOnly(this, category, category.Creator);
    }

    public void UpdateOnlyDb(
        Category category,
        SessionUserCacheItem author = null,
        bool imageWasUpdated = false,
        bool isFromModifiyRelations = false,
        CategoryChangeType type = CategoryChangeType.Update,
        bool createCategoryChange = true,
        int[] affectedParentIdsByMove = null)
    {
        base.Update(category);

        if (author != null && createCategoryChange)
        {
            categoryChangeRepo.AddUpdateEntry(this, category, author.Id, imageWasUpdated, type,
                affectedParentIdsByMove);
        }

        Flush();
        updateQuestionCountForCategory.RunOnlyDb(category);
        Task.Run(async () =>
        {
            await new MeiliSearchCategoriesDatabaseOperations()
                .UpdateAsync(category)
                .ConfigureAwait(false);
        });
    }
}