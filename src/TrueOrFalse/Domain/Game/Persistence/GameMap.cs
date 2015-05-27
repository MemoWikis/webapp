using FluentNHibernate.Mapping;

public class GameMap : ClassMap<Game>
{
    public GameMap ()
    {
        Id(x => x.Id);

        Map(x => x.WillStartAt);
        Map(x => x.MaxPlayers);
        Map(x => x.RoundCount);
        References(x => x.Creator);

        HasManyToMany(x => x.Players)
            .Table("games_to_users")
            .Cascade.SaveUpdate();
        HasManyToMany(x => x.Sets)
            .Table("games_to_sets")
            .Cascade.SaveUpdate();
        
        HasMany(x => x.Rounds).Cascade.AllDeleteOrphan();

        Map(x => x.Status);
        Map(x => x.Comment);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}