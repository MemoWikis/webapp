using FluentNHibernate.Mapping;

public class UserSkillMap : ClassMap<UserSkill>
{
    public UserSkillMap()
    {
        Id(x => x.Id);

        Map(x => x.UserId);
        Map(x => x.PageId);
        Map(x => x.EvaluationJson).Length(4000); // Allow longer JSON strings

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}
