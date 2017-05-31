using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

public class CategoryRepository : RepositoryDbBase<Category>
{
    private readonly SearchIndexCategory _searchIndexCategory;
    private readonly CategoryRelationRepo _categoryRelationRepo;

    public CategoryRepository(ISession session, SearchIndexCategory searchIndexCategory, CategoryRelationRepo categoryRelationRepo)
        : base(session){
        _searchIndexCategory = searchIndexCategory;
        _categoryRelationRepo = categoryRelationRepo;
    }

    public override void Create(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default(DateTime)))
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

    public override void Delete(Category category)
    {
        _searchIndexCategory.Delete(category);
        base.Delete(category);
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

    internal IList<Category> GetAggregatedCategories(Category category, bool includeSelf = false)
    {
        var aggregatedCategories = _categoryRelationRepo
            .GetAll()
            .Where(r => r.Category == category && r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(r => r.RelatedCategory);

        if(includeSelf)
            aggregatedCategories = aggregatedCategories.Union(new List<Category>{category});

         return aggregatedCategories.ToList();
    }

    public IList<Category> GetChildren(int categoryId)
    {

        var categoryIds = _session.CreateSQLQuery($@"SELECT Category_id
            FROM relatedcategoriestorelatedcategories
            WHERE  Related_id = {categoryId} 
            AND CategoryRelationType = {(int)CategoryRelationType.IsChildCategoryOf}").List<int>();

        return GetByIds(categoryIds.ToArray());
    }


    public IList<Category> GetChildren(
        CategoryType parentType,
        CategoryType childrenType,
        int parentId,
        String searchTerm = "")
    {

        Category relatedCategoryAlias = null; 
        Category categoryAlias = null;

        var query = Session
            .QueryOver<CategoryRelation>()
            .JoinAlias(c => c.RelatedCategory, () => relatedCategoryAlias)
            .JoinAlias(c => c.Category, () => categoryAlias)
            .Where(r =>
                r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf
                && relatedCategoryAlias.Type == parentType
                && relatedCategoryAlias.Id == parentId
                && categoryAlias.Type == childrenType);

        if (!String.IsNullOrEmpty(searchTerm))
            query.WhereRestrictionOn(r => categoryAlias.Name)
                .IsLike(searchTerm);

        return query.Select(r => r.Category).List<Category>();
    }

    public IList<Category> GetDescendants(int parentId)
    {
        var currentGeneration  = GetChildren(parentId).ToList();
        var nextGeneration = new List<Category>();
        var descendants = new List<Category>();

        while (currentGeneration.Count > 0)
        {
            descendants.AddRange(currentGeneration);

            foreach (var category in currentGeneration)
            {
                var children = GetChildren(category.Id).ToList();
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

    public int CountAggregatedSets(int categoryId)
    {
        var count = 
           _session.CreateSQLQuery($@"

            SELECT COUNT(DISTINCT(setId)) FROM
            (

                SELECT DISTINCT(cs.Set_id) setId
                FROM categories_to_sets cs
                WHERE cs.Category_id = {categoryId}

                UNION

                SELECT DISTINCT(cs.Set_id) setId
                FROM relatedcategoriestorelatedcategories rc
                INNER JOIN category c
                ON rc.Related_Id = c.Id
                INNER JOIN categories_to_sets cs
                ON c.Id = cs.Category_id
                WHERE rc.Category_id = {categoryId}
                AND rc.CategoryRelationType = {(int)CategoryRelationType.IncludesContentOf}
            ) c
        ").UniqueResult<long>();

        return (int)count;
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

            SELECT DISTINCT(qs.Question_id) questionId
	        FROM relatedcategoriestorelatedcategories rc
	        INNER JOIN category c
	        ON rc.Related_Id = c.Id
	        INNER JOIN categories_to_sets cs
	        ON c.Id = cs.Category_id
	        INNER JOIN questioninset qs
	        ON cs.Set_id = qs.Set_id
	        WHERE rc.Category_id = {categoryId}
	        AND rc.CategoryRelationType = {(int)CategoryRelationType.IncludesContentOf} 
	
	        UNION
	
	        SELECT DISTINCT(cq.Question_id) questionId 
	        FROM relatedcategoriestorelatedcategories rc
	        INNER JOIN category c
	        ON rc.Related_Id = c.Id
	        INNER JOIN categories_to_sets cs
	        ON c.Id = cs.Category_id
	        INNER JOIN categories_to_questions cq
	        ON c.Id = cq.Category_id
	        WHERE rc.Category_id = {categoryId}
	        AND rc.CategoryRelationType = {(int)CategoryRelationType.IncludesContentOf}
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
            .OrderBy(c => Math.Max(c.CountQuestions, c.CountQuestionsAggregated)).Desc
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
}