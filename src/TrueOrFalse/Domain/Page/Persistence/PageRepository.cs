using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse.Search;

public class PageRepository(
    ISession session,
    PageChangeRepo pageChangeRepo,
    UpdateQuestionCountForPage updateQuestionCountForPage,
    UserReadingRepo userReadingRepo,
    UserActivityRepo userActivityRepo)
    : RepositoryDbBase<Page>(session)
{
    /// <summary>
    ///     Add Category in Database,
    /// </summary>
    /// <param name="page"></param>
    public override void Create(Page page)
    {
        base.Create(page);
        Flush();

        UserActivityAdd.CreatedCategory(page, userReadingRepo, userActivityRepo);

        var categoryCacheItem = PageCacheItem.ToCacheCategory(page);
        EntityCache.AddOrUpdate(categoryCacheItem);

        pageChangeRepo.AddCreateEntry(this, page, page.Creator?.Id ?? -1);

        Task.Run(async () => await new MeiliSearchPagesDatabaseOperations()
            .CreateAsync(page)
            .ConfigureAwait(false));
    }

    public IList<Page> GetAllEager()
    {
        return GetByIdsEager();
    }

    public Page? GetByIdEager(int pageId) =>
        GetByIdsEager(new[] { pageId }).FirstOrDefault();


    public override void Delete(int pageId)
    {
        _session.CreateSQLQuery("DELETE FROM category WHERE Id = :pageId")
            .SetParameter("pageId", pageId)
            .ExecuteUpdate();
        ClearAllItemCache();
    }

    public IList<Page> GetByIds(List<int> questionIds)
    {
        return GetByIds(questionIds.ToArray());
    }

    public override IList<Page> GetByIds(params int[] categoryIds)
    {
        var resultTmp = base.GetByIds(categoryIds);

        var result = new List<Page>();
        for (var i = 0; i < categoryIds.Length; i++)
        {
            if (resultTmp.Any(c => c.Id == categoryIds[i]))
            {
                result.Add(resultTmp.First(c => c.Id == categoryIds[i]));
            }
        }

        return result;
    }

    public Page? GetById(int pageId)
    {
        return _session.QueryOver<Page>()
            .Where(c => c.Id == pageId)
            .SingleOrDefault();
    }

    public IList<Page> GetByIdsEager(IEnumerable<int> categoryIds = null)
    {
        var query = _session.QueryOver<Page>();
        if (categoryIds != null)
        {
            query = query.Where(Restrictions.In("Id", categoryIds.ToArray()));
        }

        var result = query
            .List()
            .GroupBy(c => c.Id)
            .Select(c => c.First())
            .ToList();

        foreach (var page in result)
        {
            NHibernateUtil.Initialize(page.Creator);
        }

        return result;
    }

    public IList<Page> GetByName(string pageTitle)
    {
        pageTitle ??= "";

        return _session.CreateQuery("from Category as c where c.Name = :pageTitle")
            .SetString("pageTitle", pageTitle)
            .List<Page>();
    }

    public IList<Page> GetCategoriesIdsForRelatedCategory(Page relatedPage)
    {
        var query = _session.QueryOver<PageRelation>()
            .Where(r => r.Parent == relatedPage);

        return query.List()
            .Select(r => r.Child)
            .ToList();
    }

    public IList<Page> GetChildren(
        PageType parentType,
        PageType childrenType,
        int parentId,
        string searchTerm = "")
    {
        Page relatedPageAlias = null;
        Page pageAlias = null;

        var query = Session
            .QueryOver<PageRelation>()
            .JoinAlias(c => c.Parent, () => relatedPageAlias)
            .JoinAlias(c => c.Child, () => pageAlias)
            .Where(r => relatedPageAlias.Type == parentType
                        && relatedPageAlias.Id == parentId
                        && pageAlias.Type == childrenType);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.WhereRestrictionOn(r => pageAlias.Name)
                .IsLike(searchTerm);
        }

        return query.Select(r => r.Child).List<Page>();
    }

    public IList<Page> GetIncludingCategories(Page page, bool includingSelf = true)
    {
        var includingCategories = GetCategoriesIdsForRelatedCategory(page);

        if (includingSelf)
        {
            includingCategories =
                includingCategories.Union(new List<Page> { page }).ToList();
        }

        return includingCategories;
    }

    /// <summary>
    ///     Update method for internal purpose, takes care that no change sets are created.
    /// </summary>
    public override void Update(Page page)
    {
        Update(page);
    }

    public void BaseUpdate(Page page)
    {
        base.Update(page);
    }

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void Update(
        Page page,
        int authorId = 0,
        bool imageWasUpdated = false,
        bool isFromModifyRelations = false,
        PageChangeType type = PageChangeType.Update,
        bool createCategoryChange = true)
    {
        base.Update(page);

        if (authorId != 0 && createCategoryChange)
        {
            pageChangeRepo.AddUpdateEntry(this, page, authorId, imageWasUpdated, type);
        }

        Flush();
        updateQuestionCountForPage.RunForJob(page, authorId);
        Task.Run(async () =>
        {
            await new MeiliSearchPagesDatabaseOperations()
                .UpdateAsync(page)
                .ConfigureAwait(false);
        });
    }

    public void CreateOnlyDb(Page page)
    {
        base.Create(page);
        Flush();

        pageChangeRepo.AddCreateEntryDbOnly(this, page, page.Creator);
    }

    public void UpdateOnlyDb(
        Page page,
        ExtendedUserCacheItem author = null,
        bool imageWasUpdated = false,
        bool isFromModifiyRelations = false,
        PageChangeType type = PageChangeType.Update,
        bool createCategoryChange = true)
    {
        base.Update(page);

        if (author != null && createCategoryChange)
        {
            pageChangeRepo.AddUpdateEntry(this, page, author.Id, imageWasUpdated, type);
        }

        Flush();
        Task.Run(async () =>
        {
            await new MeiliSearchPagesDatabaseOperations()
                .UpdateAsync(page)
                .ConfigureAwait(false);
        });
    }
}