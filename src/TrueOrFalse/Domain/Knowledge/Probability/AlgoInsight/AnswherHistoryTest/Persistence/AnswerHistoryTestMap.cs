using FluentNHibernate.Mapping;

public class AnswerHistoryTestMap : ClassMap<AnswerHistoryTest>
{
	public AnswerHistoryTestMap()
	{
		Table("AnswerHistory_Test");
	}
}
