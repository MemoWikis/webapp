using FluentNHibernate.Mapping;

public class WidgetViewMap : ClassMap<WidgetView>
{
    public WidgetViewMap()
    {
        Id(x => x.Id);

        Map(x => x.Host);
        Map(x => x.WidgetKey);

        Map(x => x.DateCreated);
    }
}