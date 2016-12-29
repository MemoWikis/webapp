﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

public class CategoryRepository : RepositoryDbBase<Category>
{
    private readonly SearchIndexCategory _searchIndexCategory;

    public CategoryRepository(ISession session, SearchIndexCategory searchIndexCategory)
        : base(session){
        _searchIndexCategory = searchIndexCategory;
    }

    public override void Create(Category category)
    {
        foreach (var related in category.ParentCategories.Where(x => x.DateCreated == default(DateTime)))
            related.DateModified = related.DateCreated = DateTime.Now;
        base.Create(category);
        Flush();
        UserActivityAdd.CreatedCategory(category);
        _searchIndexCategory.Update(category);
    }

    public override void Update(Category category)
    {
        _searchIndexCategory.Update(category);
        base.Update(category);
        Flush();
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

    public IList<Category> GetChildren(
        CategoryType parentType, 
        CategoryType childrenType, 
        int parentId, 
        String searchTerm = "")
    {
        var query = Session
            .QueryOver<Category>()
            .Where(c => c.Type == childrenType);

        if (!String.IsNullOrEmpty(searchTerm))
            query.WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm);

        query.JoinQueryOver<Category>(c => c.ParentCategories)
            .Where(c =>
                c.Type == parentType &&
                c.Id == parentId
            );
            
        return query.List<Category>();            
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
            .OrderBy(c => c.CountQuestions).Desc
            .Take(amount)
            .List();
    }

    public IEnumerable<Category> GetMostRecent_WithAtLeast3Questions(int amount)
    {
        return _session
            .QueryOver<Category>()
            .Where(c => c.CountQuestions > 3)
            .OrderBy(c => c.DateCreated)
            .Desc
            .Take(amount)
            .List();
    }

    public IList<Question> GetRandomQuestions(Category category, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        var result = category.FeaturedSets.Count > 0 
            ? category.FeaturedSets.SelectMany(s => s.Questions()).Distinct().ToList()
            : Sl.R<QuestionRepo>().GetForCategory(category.Id, category.CountQuestions, _userSession.UserId);

        if ((excludeQuestionIds != null) && (excludeQuestionIds.Count > 0) && (result.Count > amount))
        {
            var fillUpQuestions = result.Where(q => excludeQuestionIds.Contains(q.Id)).ToList();
            result = result.Where(q => !excludeQuestionIds.Contains(q.Id)).ToList();
            if (ignoreExclusionIfNotEnoughQuestions && (result.Count < amount))
            {
                //possible improvement: if questions are to be reasked, prioritize those that have been answered wrong by the user.
                var fillUpAmount = amount - result.Count;
                fillUpQuestions.Shuffle();
                ((List<Question>)result).AddRange(fillUpQuestions.Take(fillUpAmount).ToList());
            }
        }
        result.Shuffle();
        return result.Take(amount).ToList();
    }

    public bool Exists(string categoryName)
    {
        return GetByName(categoryName).Any(x => x.Type == CategoryType.Standard);
    }

    public IList<Category> GetChildren(int categoryId)
    {
        var categoryIds = _session.CreateSQLQuery(@"SELECT Category_id
            FROM relatedcategoriestorelatedcategories
            WHERE  Related_id = " + categoryId).List<int>();

        return GetByIds(categoryIds.ToArray());
    }
}