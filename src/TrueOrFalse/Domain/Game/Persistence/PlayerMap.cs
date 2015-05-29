using FluentNHibernate.Mapping;
using NHibernate.Proxy;

class PlayerMap : ClassMap<Player>
{
    public PlayerMap()
    {
        Table("game_player");

        Id(x => x.Id);

        References(x => x.Game);
        References(x => x.User);

        HasMany(x => x.Answers).Cascade.None();

        Map(x => x.Position);
        Map(x => x.IsCreator);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}