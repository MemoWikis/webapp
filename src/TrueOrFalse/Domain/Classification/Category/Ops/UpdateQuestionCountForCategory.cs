using System.Collections.Generic;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    public void All()
    {
        Run(Sl.R<CategoryRepository>().GetAll());
    }

    public void Run(IList<Category> categories)
    {
        foreach (var category in categories)
        {
            var query =
                "UPDATE category SET CountQuestions = " +
                " (SELECT COUNT(*) " +
                "  FROM categories_to_questions as cq" +
                "  LEFT JOIN  question as q" +
                "  ON cq.Question_id = q.Id " +
                "  WHERE Category_id = category.Id" +
                "  AND q.Visibility = 0) + " +
                " (SELECT COUNT(*) " +
                "  FROM reference r " +
                "  LEFT JOIN question as q" +
                "  ON r.Question_id = q.Id " +
                "  WHERE r.Category_id = category.Id " +
                "  AND q.Visibility = 0) " +
                "WHERE Id = " + category.Id;

            Sl.R<ISession>().CreateSQLQuery(query).ExecuteUpdate();                
        }
    }
}