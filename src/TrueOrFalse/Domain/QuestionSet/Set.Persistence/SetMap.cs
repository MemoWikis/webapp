using FluentNHibernate.Mapping;

public class SetMap : ClassMap<Set>
{
    public SetMap()
    {
        Table("QuestionSet");
        Id(x => x.Id);
        Map(x => x.Name).Length(100);
        Map(x => x.Text).Length(Constants.VarCharMaxLength);
        Map(x => x.VideoUrl);

        Map(x => x.TotalRelevancePersonalAvg);
        Map(x => x.TotalRelevancePersonalEntries);

        HasMany(x => x.QuestionsInSet).Table("questionInSet").Cascade.All().OrderBy("Sort");
        HasManyToMany(x => x.Categories).Table("categories_to_sets").Cascade.SaveUpdate();

        References(x => x.CopiedFrom).Column("CopiedFrom").Cascade.None(); //if parent is deleted, child remains and its column "CopiedFrom" is set to NULL
        HasMany(x => x.CopiedInstances).Cascade.None().KeyColumn("CopiedFrom");

        References(x => x.Creator);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}