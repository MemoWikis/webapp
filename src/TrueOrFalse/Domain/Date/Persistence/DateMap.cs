using FluentNHibernate.Mapping;

public class DateMap : ClassMap<Date>
{
    public DateMap()
    {
        Id(x => x.Id);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}