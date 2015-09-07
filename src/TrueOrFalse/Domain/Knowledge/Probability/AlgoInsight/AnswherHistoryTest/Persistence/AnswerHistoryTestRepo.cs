using NHibernate;

public class AnswerHistoryTestRepo : RepositoryDbBase<AnswerHistoryTest>
{
	public AnswerHistoryTestRepo(ISession session) : base(session)
	{
	}
}
