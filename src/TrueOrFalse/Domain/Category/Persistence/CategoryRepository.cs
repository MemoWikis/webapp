using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse.Search;

public class CategoryRepository : RepositoryDbBase<Category>
{
    public enum CreateDeleteUpdate
    {
        Create = 1,
        Delete = 2,
        Update = 3
    }

    public const int AllgemeinwissenId = 709;
    public const int SchuleId = 682;
    public const int StudiumId = 687;
    public const int ZertifikateId = 689;
    private readonly SolrSearchIndexCategory _solrSearchIndexCategory;
    private readonly bool _isSolrActive;

    public CategoryRepository(ISession session, SolrSearchIndexCategory solrSearchIndexCategory)
        : base(session)
    {
        _isSolrActive = Settings.UseMeiliSearch() == false;

        if (_isSolrActive)
        {
            _solrSearchIndexCategory = solrSearchIndexCategory;
        }
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

        if (_isSolrActive)
        {
            _solrSearchIndexCategory.Update(category);
        }

        var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(categoryCacheItem);

        Sl.CategoryChangeRepo.AddCreateEntry(category, category.Creator?.Id ?? -1);

        GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(categoryCacheItem);
        UpdateCachedData(categoryCacheItem, CreateDeleteUpdate.Create);

        if (category.ParentCategories().Count != 1)
        {
            Logg.r().Warning("the parentcounter is != 1");
        }

        var parentCategories = category.ParentCategories();

        if (parentCategories.Count != 0)
        {
            Sl.CategoryChangeRepo.AddUpdateEntry(category, category.Creator?.Id ?? default, false,
                CategoryChangeType.Relations);
        }

        var result = Task.Run(async () => await new MeiliSearchCategoriesDatabaseOperations()
            .CreateAsync(category)
            .ConfigureAwait(false));
    }

    public void CreateOnlyDb(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default))
        {
            related.DateModified = related.DateCreated = DateTime.UtcNow.AddHours(2);
        }

        base.Create(category);
        Flush();
        if (_isSolrActive)
        {
            _solrSearchIndexCategory.Update(category);
        }

        Sl.CategoryChangeRepo.AddCreateEntryDbOnly(category, category.Creator);
    }

    public override void Delete(Category category)
    {
        if (_isSolrActive)
        {
            _solrSearchIndexCategory.Delete(category);
        }

        base.Delete(category);
        EntityCache.Remove(EntityCache.GetCategory(category));
        Task.Run(async () =>
        {
            await new MeiliSearchCategoriesDatabaseOperations()
                .DeleteAsync(category)
                .ConfigureAwait(false);
        });
    }

    public override void DeleteWithoutFlush(Category category)
    {
        if (_isSolrActive)
        {
            _solrSearchIndexCategory.Delete(category);
        }

        base.DeleteWithoutFlush(category);
        EntityCache.Remove(EntityCache.GetCategory(category.Id));
        SessionUserCache.RemoveAllForCategory(category.Id);
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

    public IEnumerable<int> GetRootCategoryInts()
    {
        return GetRootCategoryListIds();
    }

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

    public int TotalCategoryCount()
    {
        return _session.QueryOver<Category>()
            .RowCount();
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
        if (!isFromModifiyRelations && _isSolrActive)
        {
            _solrSearchIndexCategory.Update(category);
        }

        base.Update(category);

        if (author != null && createCategoryChange)
        {
            Sl.CategoryChangeRepo.AddUpdateEntry(category, author.Id, imageWasUpdated, type, affectedParentIdsByMove);
        }

        Flush();
        Sl.R<UpdateQuestionCountForCategory>().Run(category);
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
                {
                    EntityCache.GetCategory(id).CachedData
                        .AddChildId(categoryCacheItem.Id);
                }

                foreach (var id in exceptIdsToDelete1)
                {
                    EntityCache.GetCategory(id).CachedData.RemoveChildId(categoryCacheItem.Id);
                }
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
}