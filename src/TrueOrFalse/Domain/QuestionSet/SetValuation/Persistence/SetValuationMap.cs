using FluentNHibernate.Mapping;

public class SetValuationMap : ClassMap<SetValuation>
{
    public SetValuationMap()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.SetId);

        Map(x => x.RelevancePersonal);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}