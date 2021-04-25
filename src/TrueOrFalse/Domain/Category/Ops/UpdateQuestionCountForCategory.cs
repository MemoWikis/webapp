using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionRepo _questionRepository;

    public UpdateQuestionCountForCategory(CategoryRepository categoryRepository, QuestionRepo questionRepo)
    {
        _categoryRepository = categoryRepository;
        _questionRepository = questionRepo;
    }

    public void Run(Category category)
    {
        category.CountQuestions = _questionRepository.GetForCategory(category.Id).Count;
        category.UpdateCountQuestionsAggregated();
    }

    public void Run(IList<Category> categories)
    {
        foreach (var category in categories) 
            Run(category);
    }

    public void Run(IList<int> categoryIds)
    {
        foreach (var categoryId in categoryIds) 
            Run(_categoryRepository.GetById(categoryId));
    }

    public void All() => RunWithSql(_categoryRepository.GetAll());

    public void RunWithSql(IList<Category> categories) => RunWithSql(categories.Select(c => c.Id));

    public void RunWithSql(IEnumerable<int> categoryIds)
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