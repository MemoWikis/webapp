using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepository;

    public UpdateQuestionCountForCategory(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public void All() => Run(_categoryRepository.GetAll());

    public void Run(IList<Category> categories) => Run(categories.Select(c => c.Id));

    public void Run(IEnumerable<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            var query = $@"

                UPDATE category c SET CountQuestions =  
                (
	                SELECT COUNT(ID) 
	                FROM
	                (
		                (
			                SELECT q.ID  
			                FROM categories_to_questions as cq  
			                LEFT JOIN  question as q
			                ON cq.Question_id = q.Id   
			                WHERE Category_id = {categoryId}
			                AND q.Visibility = 0
		                )
		                UNION DISTINCT
		                (
			                SELECT q.ID
			                FROM reference r   
			                LEFT JOIN question as q  
			                ON r.Question_id = q.Id   
			                WHERE r.Category_id = {categoryId}
			                AND q.Visibility = 0
		                )
	                )I_was_here
                ) WHERE c.Id = {categoryId}";

            Sl.R<ISession>().CreateSQLQuery(query).ExecuteUpdate();
        }
    }

    public void RunAggregated(IEnumerable<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            var query = $@"

                UPDATE category c SET CountQuestions =  
                (
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
                ) WHERE c.Id = {categoryId}";

            Sl.R<ISession>().CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}