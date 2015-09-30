using FluentNHibernate.Mapping;

public class QuestionFeatureMap : ClassMap<QuestionFeature>
{
    public QuestionFeatureMap()
    {
        Id(x => x.Id);

        Map(x => x.Id2);

        Map(x => x.Name);
        Map(x => x.Description);

        HasManyToMany(x => x.Questions).
            Table("questionFeature_to_question");

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}