using FluentNHibernate.Mapping;

public class AnswerFeatureMap : ClassMap<AnswerFeature>
{
    public AnswerFeatureMap()
    {
        Id(x => x.Id);

        Map(x => x.Id2);

        Map(x => x.Group, "GroupName");
        Map(x => x.Name);
        Map(x => x.Description);

        HasManyToMany(x => x.Answers).Table("answerFeature_to_answer");

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}