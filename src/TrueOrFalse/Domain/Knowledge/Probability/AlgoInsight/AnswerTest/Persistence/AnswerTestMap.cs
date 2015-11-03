using FluentNHibernate.Mapping;

public class AnswerTestMap : ClassMap<AnswerTest>
{
	public AnswerTestMap()
	{
		Table("AnswerHistory_Test");

		Id(x => x.Id);

		Map(x => x.AlgoId);
		References(x => x.Answer);

        Map(x => x.Probability);
        Map(x => x.IsCorrect);
    }
}
