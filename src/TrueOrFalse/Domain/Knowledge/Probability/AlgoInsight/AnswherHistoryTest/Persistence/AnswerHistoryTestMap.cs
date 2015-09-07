using FluentNHibernate.Mapping;

public class AnswerHistoryTestMap : ClassMap<AnswerHistoryTest>
{
	public AnswerHistoryTestMap()
	{
		Table("AnswerHistory_Test");

		Id(x => x.Id);

		Map(x => x.AlgoId);
		References(x => x.AnswerHistory);

        Map(x => x.Probability);
        Map(x => x.IsCorrect);
    }
}
