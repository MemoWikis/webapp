using NUnit.Framework;

public class Widget_view_tests : BaseTest
{
    [Test]
    public void Should_write_widget_view()
    {       
        Sl.WidgetViewRepo.Create(new WidgetView { Host = "foo.de", WidgetKey = "w1", WidgetType = WidgetType.Question});
        Sl.WidgetViewRepo.Create(new WidgetView { Host = "foo.de", WidgetKey = "w1", WidgetType = WidgetType.Question });
        Sl.WidgetViewRepo.Create(new WidgetView { Host = "foo.de", WidgetKey = "w1", WidgetType = WidgetType.Question });

        Sl.WidgetViewRepo.Create(new WidgetView { Host = "bar.de", WidgetKey = "b1", WidgetType = WidgetType.Question });
        Sl.WidgetViewRepo.Create(new WidgetView { Host = "bar.de", WidgetKey = "b2", WidgetType = WidgetType.Question });

        RecycleContainer();

        Assert.That(Sl.WidgetViewRepo.GetAll().Count, Is.EqualTo(5));
    }
}