using FluentNHibernate.Mapping;

public class QuestionViewMap : ClassMap<QuestionView>
{
    public QuestionViewMap()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.QuestionId);
        Map(x => x.DateCreated);
    }
}