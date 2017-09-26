using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Util;
using Seedworks.Lib.Persistence;

public class WidgetViewRepo : RepositoryDb<WidgetView>
{
    public WidgetViewRepo(ISession session) : base(session)
    {
    }

    public IList<WidgetViewsPerMonthAndKeyResult> GetWidgetViewsPerMonthAndKey(string host)
    {
        var result = new List<WidgetViewsPerMonthAndKeyResult>();
        _session.QueryOver<WidgetView>()
            .Where(v => v.Host == host)
            .List()
            .GroupBy(v => new DateTime(v.DateCreated.Year, v.DateCreated.Month, 1))
            .ForEach(month =>
            {
                var widgetViews = new Dictionary<string, int>();
                month.GroupBy(v => v.WidgetKey)
                    .ForEach(w => widgetViews.Add(w.Key, w.Count()));
                result.Add(new WidgetViewsPerMonthAndKeyResult { Month = month.Key, ViewsPerWidgetKey = widgetViews});
            });

        return result;
    }

    public IList<string> GetWidgetKeys(string host)
    {
        var result = new List<string>();
        _session.QueryOver<WidgetView>()
            .Where(v => v.Host == host)
            .List()
            .GroupBy(v => v.WidgetKey)
            .ForEach(k => result.Add(k.Key));
        return result;
    }

    public IList<WidgetDetailViewsPerMonthAndTypeResult> GetWidgetDetailViewsPerMonthAndType(string host,
        string widgetKey)
    {
        var result = new List<WidgetDetailViewsPerMonthAndTypeResult>();
        _session.QueryOver<WidgetView>()
            .Where(v => v.Host == host)
            .And(v => v.WidgetKey == widgetKey)
            .List()
            .GroupBy(v => new DateTime(v.DateCreated.Year, v.DateCreated.Month, 1))
            .ForEach(month =>
            {
                var widgetViews = new Dictionary<WidgetType, int>();
                month.GroupBy(v => v.WidgetType)
                    .ForEach(w => widgetViews.Add(w.Key, w.Count()));
                result.Add(new WidgetDetailViewsPerMonthAndTypeResult { Month = month.Key, ViewsPerWidgetType = widgetViews });
            });
        return result;
    }

    public IList<WidgetType> GetWidgetTypes(string host, string widgetKey)
    {
        var result = new List<WidgetType>();
        _session.QueryOver<WidgetView>()
            .Where(v => v.Host == host)
            .And(v => v.WidgetKey == widgetKey)
            .List()
            .GroupBy(v => v.WidgetType)
            .ForEach(k => result.Add(k.Key));
        return result;
    }
}