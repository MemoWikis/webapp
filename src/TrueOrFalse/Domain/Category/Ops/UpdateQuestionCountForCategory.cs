using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly QuestionReadingRepo _questionReadingRepository;
    private readonly SessionUser _sessionUser;
    private readonly ISession _nhinbernateSession;

    public UpdateQuestionCountForCategory(
        QuestionReadingRepo questionReadingRepo,
        SessionUser sessionUser,
        ISession nhinbernateSession)
    {
        _questionReadingRepository = questionReadingRepo;
        _sessionUser = sessionUser;
        _nhinbernateSession = nhinbernateSession;
    }

    public void All(CategoryRepository categoryRepository)
    {
        RunWithSql(categoryRepository.GetAll().Select(c => c.Id));
    }

    public void Run(Category category)
    {
        category.CountQuestions = _questionReadingRepository.GetForCategory(category.Id).Count;
        category.UpdateCountQuestionsAggregated(_sessionUser.UserId);
    }

    public void Run(IList<Category> categories)
    {
        foreach (var category in categories)
        {
            Run(category);
        }
    }

    private void RunWithSql(IEnumerable<int> categoryIds)
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
	                )
                ) WHERE c.Id = {categoryId}";

            _nhinbernateSession.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}