using NHibernate;

public class AnswerHistoryTestRepo : RepositoryDbBase<AnswerHistoryTest>
{
	public AnswerHistoryTestRepo(ISession session) : base(session)
	{
	}

	public void TruncateTable()
	{
		_session.CreateSQLQuery("truncate table answerhistory_test").ExecuteUpdate();
	}
}
