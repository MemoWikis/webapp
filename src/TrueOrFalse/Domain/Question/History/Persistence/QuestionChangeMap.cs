using FluentNHibernate.Mapping;

public class QuestionChangeMap : ClassMap<QuestionChange>
{
    public QuestionChangeMap()
    {
        Id(x => x.Id);

        References(x => x.Question);

        Map(x => x.Data);
        Map(x => x.ShowInSidebar);

        Map(x => x.Type);
        Map(x => x.DataVersion);
        Map(x => x.DateCreated);

        References(x => x.Author);
    }
}

