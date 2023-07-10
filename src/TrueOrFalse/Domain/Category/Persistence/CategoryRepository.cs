using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse.Search;

public class CategoryRepository : RepositoryDbBase<Category>, IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;

    public enum CreateDeleteUpdate
    {
        Create = 1,
        Delete = 2,
        Update = 3
    }

    public CategoryRepository(
        ISession session,
        PermissionCheck permissionCheck,
        CategoryChangeRepo categoryChangeRepo,
        CategoryValuationRepo categoryValuationRepo,
        UpdateQuestionCountForCategory updateQuestionCountForCategory)
        : base(session)
    {
        _permissionCheck = permissionCheck;
        _categoryChangeRepo = categoryChangeRepo;
        _categoryValuationRepo = categoryValuationRepo;
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
    }

    /// <summary>
    ///     Add Category in Database,
    /// </summary>
    /// <param name="category"></param>
    public override void Create(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default))
        {
            related.DateModified = related.DateCreated = DateTime.Now;
        }

        base.Create(category);
        Flush();

        UserActivityAdd.CreatedCategory(category);

        var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(categoryCacheItem);

        _categoryChangeRepo.AddCreateEntry(this, category, category.Creator?.Id ?? -1);

        GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(categoryCacheItem, category.Creator.Id);
        EntityCache.UpdateCachedData(categoryCacheItem, CreateDeleteUpdate.Create);

        if (category.ParentCategories().Count != 1)
        {
            Logg.r().Warning("the parentcounter is != 1");
        }

        var parentCategories = category.ParentCategories();

        if (parentCategories.Count != 0)
        {
            _categoryChangeRepo.AddUpdateEntry(this, category, category.Creator?.Id ?? default, false,
                 CategoryChangeType.Relations);
        }

        var result = Task.Run(async () => await new MeiliSearchCategoriesDatabaseOperations()
            .CreateAsync(category)
            .ConfigureAwait(false));
    }

    //todo (Meili) die Stelle nochmal anschauen
    public void CreateOnlyDb(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default))
        {
            related.DateModified = related.DateCreated = DateTime.UtcNow.AddHours(2);
        }

        base.Create(category);
        Flush();

        _categoryChangeRepo.AddCreateEntryDbOnly(this, category, category.Creator);
    }

    public void Delete(Category category, int userId)
    {

        base.Delete(category);
        EntityCache.Remove(EntityCache.GetCategory(category), _permissionCheck, userId);
        Task.Run(async () =>
        {
            await new MeiliSearchCategoriesDatabaseOperations()
                .DeleteAsync(category)
                .ConfigureAwait(false);
        });
    }

    public void DeleteWithoutFlush(Category category, int userId)
    {
        base.DeleteWithoutFlush(category);
        EntityCache.Remove(EntityCache.GetCategory(category.Id), _permissionCheck, userId);
        SessionUserCache.RemoveAllForCategory(category.Id, _categoryValuationRepo);
        Task.Run(async () =>
        {
            await new MeiliSearchCategoriesDatabaseOperations()
                .DeleteAsync(category)
                .ConfigureAwait(false);
        });
    }

    public bool Exists(string categoryName)
    {
        return GetByName(categoryName).Any(x => x.Type == CategoryType.Standard);
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

    public IList<Category> GetByIdsFromString(string idsString)
    {
        return idsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x))
            .Select(GetById)
            .Where(set => set != null)
            .ToList();
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
            .Where(r => r.RelatedCategory == relatedCategory);

        return query.List()
            .Select(r => r.Category)
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
            .JoinAlias(c => c.RelatedCategory, () => relatedCategoryAlias)
            .JoinAlias(c => c.Category, () => categoryAlias)
            .Where(r => relatedCategoryAlias.Type == parentType
                        && relatedCategoryAlias.Id == parentId
                        && categoryAlias.Type == childrenType);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.WhereRestrictionOn(r => categoryAlias.Name)
                .IsLike(searchTerm);
        }

        return query.Select(r => r.Category).List<Category>();
    }

    public IList<Category> GetIncludingCategories(Category category, bool includingSelf = true)
    {
        var includingCategories = GetCategoriesIdsForRelatedCategory(category);

        if (includingSelf)
        {
            includingCategories = includingCategories.Union(new List<Category> { category }).ToList();
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
            _categoryChangeRepo.AddUpdateEntry(this, category, author.Id, imageWasUpdated, type, affectedParentIdsByMove);
        }

        Flush();
        _updateQuestionCountForCategory.Run(category);
        Task.Run(async () =>
        {
            await new MeiliSearchCategoriesDatabaseOperations()
                .UpdateAsync(category)
                .ConfigureAwait(false);
        });
    }

    public void UpdateAuthors(Category category)
    {
        var sql = "UPDATE category " +
                  "SET AuthorIds = " + "'" + category.AuthorIds + "'" +
                  " WHERE Id = " + category.Id;
        _session.CreateSQLQuery(sql).ExecuteUpdate();
    }


}