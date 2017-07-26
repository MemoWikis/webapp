public class SaveWidgetView
{
    public static WidgetView Run(string host, string widgetKey, WidgetType widgetType, int entityId)
    {
        var widgetView = new WidgetView
        {
            Host = host,
            WidgetKey = widgetKey,
            WidgetType = widgetType,
            EntityId = entityId
        };
        Sl.WidgetViewRepo.Create(widgetView);
        return widgetView;
    }
}
