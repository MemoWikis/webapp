using FluentNHibernate.Mapping;

public class AnswerTestMap : ClassMap<AnswerTest>
{
	public AnswerTestMap()
	{
		Table("answer_test");

		Id(x => x.Id);

		Map(x => x.AlgoId);
		References(x => x.Answer);

        Map(x => x.Probability);
        Map(x => x.IsCorrect);
    }
}
