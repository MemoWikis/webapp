using FluentNHibernate.Mapping;

public class CategoryValuationMap : ClassMap<CategoryValuation>
{
    public CategoryValuationMap()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.CategoryId);

        Map(x => x.RelevancePersonal);

        Map(x => x.CountNotLearned);
        Map(x => x.CountNeedsLearning);
        Map(x => x.CountNeedsConsolidation);
        Map(x => x.CountSolid);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}