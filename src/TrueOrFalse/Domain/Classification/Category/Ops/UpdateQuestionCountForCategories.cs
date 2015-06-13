using NHibernate;

public class UpdateQuestionCountForAllCategories : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public UpdateQuestionCountForAllCategories(ISession session){
        _session = session;
    }

    public void Run()
    {
        var query =
            "UPDATE category SET CountQuestions = " +
            "(SELECT COUNT(*) FROM categoriestoquestions WHERE Category_id = category.Id)";

        _session.CreateSQLQuery(query).ExecuteUpdate();
    }
}