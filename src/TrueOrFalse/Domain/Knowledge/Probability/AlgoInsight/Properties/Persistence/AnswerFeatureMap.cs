using FluentNHibernate.Mapping;

public class AnswerFeatureMap : ClassMap<AnswerFeature>
{
    public AnswerFeatureMap()
    {
        Id(x => x.Id);

        Map(x => x.Id2);
        Map(x => x.Name);

        HasManyToMany(x => x.AnswerHistories).
            Table("answerFeature_to_answerHistory");

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}