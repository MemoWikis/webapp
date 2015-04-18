﻿using FluentNHibernate.Mapping;

public class ReferenceMap : ClassMap<Reference>
{
    public ReferenceMap()
    {
        Id(x => x.Id);

        References(x => x.Question).Cascade.None();
        References(x => x.Category);

        Map(x => x.ReferenceType);
        Map(x => x.AdditionalInfo);
        Map(x => x.ReferenceText);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}