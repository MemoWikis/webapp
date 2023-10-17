using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionRepo _questionRepository;
    private readonly SessionUser _sessionUser;

    public UpdateQuestionCountForCategory(CategoryRepository categoryRepository, QuestionRepo questionRepo, SessionUser sessionUser)
    {
        _categoryRepository = categoryRepository;
        _questionRepository = questionRepo;
        _sessionUser = sessionUser;
    }

    public void All()
    {
        RunWithSql(_categoryRepository.GetAll());
    }

    public void Run(Category category)
    {
        category.CountQuestions = _questionRepository.GetForCategory(category.Id).Count;
        category.UpdateCountQuestionsAggregated(_sessionUser.UserId);
    }

    public void RunOnlyDb(Category category)
    {
        category.CountQuestions = _questionRepository.GetForCategory(category.Id).Count;
    }

    public void Run(IList<Category> categories)
    {
        foreach (var category in categories)
        {
            Run(category);
        }
    }

    public void Run(IList<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            Run(_categoryRepository.GetById(categoryId));
        }
    }

    public void RunWithSql(IList<Category> categories)
    {
        RunWithSql(categories.Select(c => c.Id));
    }

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
}