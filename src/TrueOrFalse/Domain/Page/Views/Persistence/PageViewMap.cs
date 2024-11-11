﻿using FluentNHibernate.Mapping;

public class PageViewMap : ClassMap<PageView>
{
    public PageViewMap()
    {
        Id(x => x.Id);

        References(x => x.Page).Cascade.None();
        References(x => x.User).Cascade.None();

        Map(x => x.UserAgent);

        Map(x => x.DateCreated);
        Map(x => x.DateOnly).ReadOnly();
    }
}