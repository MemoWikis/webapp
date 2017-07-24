public class SaveWidgetView
{
    public static WidgetView Run(string host, string widgetKey, WidgetType widgetType)
    {
        var widgetView = new WidgetView
        {
            Host = host,
            WidgetKey = widgetKey,
            WidgetType = widgetType
        };
        Sl.WidgetViewRepo.Create(widgetView);
        return widgetView;
    }
}
