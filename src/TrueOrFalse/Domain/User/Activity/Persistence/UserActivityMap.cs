﻿using FluentNHibernate.Mapping;

public class UserActivityMap : ClassMap<UserActivity>
{
    public UserActivityMap()
    {
        Id(x => x.Id);

        References(x => x.UserConcerned);
        Map(x => x.At);
        Map(x => x.Type).CustomType<UserActivityType>();
        References(x => x.Question);
        References(x => x.Page).Column("Page_id");
        References(x => x.UserIsFollowed);
        References(x => x.UserCauser);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}
