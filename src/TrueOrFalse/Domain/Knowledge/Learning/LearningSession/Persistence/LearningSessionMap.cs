using FluentNHibernate.Mapping;

public class LearningSessionMap : ClassMap<LearningSession>
{
    public LearningSessionMap()
    {
        Id(x => x.Id);

        Map(x => x.StepsJson).CustomSqlType("varchar(8000)");

        References(x => x.User);

        References(x => x.SetToLearn);
        Map(x => x.SetsToLearnIdsString);
        Map(x => x.SetListTitle);
        References(x => x.CategoryToLearn);

        Map(x => x.IsWishSession);

        Map(x => x.IsCompleted);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}