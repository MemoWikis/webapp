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
                
        References(x => x.Creator);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}