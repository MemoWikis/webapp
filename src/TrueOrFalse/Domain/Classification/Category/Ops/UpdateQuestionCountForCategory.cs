using System.Collections.Generic;
using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public UpdateQuestionCountForCategory(ISession session){
        _session = session;
    }

    public void Run(IList<Category> categories )
    {
        foreach (var category in categories)
        {
            var query =
                "UPDATE category SET CountQuestions = " +
                " (SELECT COUNT(*) " +
                "  FROM categoriestoquestions as cq" +
                "  LEFT JOIN  question as q" +
                "  ON cq.Question_id = q.Id " +
                "  WHERE Category_id = category.Id" +
                "  AND q.Visibility = 0) " +
                "WHERE Id = " + category.Id;

            _session.CreateSQLQuery(query).ExecuteUpdate();                
        }
    }
}