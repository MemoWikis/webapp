using FluentNHibernate.Mapping;

public class PasswordRecoveryTokenMap : ClassMap<PasswordRecoveryToken>
{
    public PasswordRecoveryTokenMap()
    {
        Id(x => x.Id);
        Map(x => x.Token);
        Map(x => x.Email);
        Map(x => x.DateModified);
        Map(x => x.DateCreated);
    }
}