using FluentNHibernate.Mapping;

public class QuestionChangeMap : ClassMap<QuestionChange>
{
    public QuestionChangeMap()
    {
        Id(x => x.Id);

        References(x => x.Question);

        Map(x => x.Data).CustomSqlType("text");
        Map(x => x.ShowInSidebar);

        Map(x => x.Type).CustomType<QuestionChangeType>();
        Map(x => x.DataVersion);
        Map(x => x.DateCreated);

        Map(x => x.AuthorId).Column("Author_Id");
    }
}

