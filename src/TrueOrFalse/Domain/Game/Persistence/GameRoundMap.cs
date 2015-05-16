using FluentNHibernate.Mapping;

public class GameRoundMap : ClassMap<GameRound>
{
    public GameRoundMap()
    {
        Id(x => x.Id);

        References(x => x.Question);
        References(x => x.Set);
        References(x => x.Game).Cascade.None();

        Map(x => x.Status);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}