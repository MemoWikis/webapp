using FluentNHibernate.Mapping;

public class SharesMap : ClassMap<Share>
{
    public SharesMap()
    {
        Table("share_info");
        Id(x => x.Id);
        Map(x => x.PageId);
        Map(x => x.UserId).Nullable();
        Map(x => x.Token).Not.Nullable();
        Map(x => x.Permission).CustomType<SharePermission>();
        Map(x => x.GrantedBy).Not.Nullable();
    }
}