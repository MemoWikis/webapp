using FluentNHibernate.Mapping;

public class SharesMap : ClassMap<Share>
{
    public SharesMap()
    {
        Table("share_info");
        Id(x => x.Id);
        Map(x => x.PageId);
        References(x => x.User).Column("UserId").Cascade.None();
        Map(x => x.Token).Not.Nullable();
        Map(x => x.Permission).CustomType<SharePermission>();
        Map(x => x.GrantedBy).Not.Nullable();
    }
}