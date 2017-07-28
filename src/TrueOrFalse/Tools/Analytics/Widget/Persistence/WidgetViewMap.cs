using FluentNHibernate.Mapping;

public class WidgetViewMap : ClassMap<WidgetView>
{
    public WidgetViewMap()
    {
        Id(x => x.Id);

        Map(x => x.Host);
        Map(x => x.WidgetKey);
        Map(x => x.WidgetType);

        Map(x => x.EntityId);

        Map(x => x.DateCreated);
    }
}