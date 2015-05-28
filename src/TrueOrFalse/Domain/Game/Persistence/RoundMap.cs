using FluentNHibernate.Mapping;

public class RoundMap : ClassMap<Round>
{
    public RoundMap()
    {
        Table("game_round");

        Id(x => x.Id);

        References(x => x.Question);
        References(x => x.Set);
        References(x => x.Game).Cascade.None();

        HasMany(x => x.Answers).Cascade.All();

        Map(x => x.Status);

        Map(x => x.Number);

        Map(x => x.StartTime);
        Map(x => x.EndTime);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}