using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    public void All()
    {
        Run(Sl.R<CategoryRepository>().GetAll());
    }

    public void Run(IList<Category> categories)
    {
        Run(categories.Select(c => c.Id));
    }

    public void Run(IEnumerable<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
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
                "WHERE Id = " + categoryId;

            Sl.R<ISession>().CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}