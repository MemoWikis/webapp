﻿using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ISession = NHibernate.ISession;

public class CategoryDeleter : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly SessionUser _sessionUser;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly CategoryValuationWritingRepo _categoryValuationWritingRepo;
    private readonly CategoryValuationReadingRepo _categoryValuationReading;
    private readonly IMemoryCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;

    public CategoryDeleter(
        ISession session,
        SessionUser sessionUser,
        UserActivityRepo userActivityRepo,
        CategoryRepository categoryRepository,
        CategoryChangeRepo categoryChangeRepo,
        CategoryValuationWritingRepo categoryValuationWritingRepo,
        PermissionCheck permissionCheck,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        CategoryValuationReadingRepo categoryValuationReading,
        IMemoryCache cache,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache)
    {
        _session = session;
        _sessionUser = sessionUser;
        _userActivityRepo = userActivityRepo;
        _categoryRepository = categoryRepository;
        _categoryChangeRepo = categoryChangeRepo;
        _categoryValuationWritingRepo = categoryValuationWritingRepo;
        _permissionCheck = permissionCheck;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _categoryValuationReading = categoryValuationReading;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
    }
    ///todo:(DaMa)  Revise: Wrong place for SQL commands.
    public HasDeleted Run(Category category, int userId, bool isTestCase = false)
    {
        var categoryCacheItem = EntityCache.GetCategory(category.Id);
        var hasDeleted = new HasDeleted();

        if (!_sessionUser.IsInstallationAdmin && _sessionUser.UserId != categoryCacheItem.Creator.Id)
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        if (!isTestCase)
        {
            _session.CreateSQLQuery(
                "DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " +
                                    category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + category.Id)
                .ExecuteUpdate();
        }

        _userActivityRepo.DeleteForCategory(category.Id);

        _categoryChangeRepo.AddDeleteEntry(category, userId);
        _categoryValuationWritingRepo.DeleteCategoryValuation(category.Id);
        _categoryRepository.Delete(category);

        ModifyRelationsEntityCache.DeleteIncludetContentOf(categoryCacheItem);
        EntityCache.Remove(categoryCacheItem, _permissionCheck, userId);
        _sessionUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        hasDeleted.DeletedSuccessful = true;
        return hasDeleted;
    }

    public class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
    }
}