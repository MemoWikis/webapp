using Seedworks.Lib.Persistence;

public class ShareInfoCacheItem : IPersistable
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public int? UserId { get; set; }
    public string Token { get; set; } = "";
    public SharePermission Permission { get; set; } = SharePermission.View;
    public int GrantedBy { get; set; }

    public static ShareInfoCacheItem ToCacheItem(ShareInfo dbItem)
    {
        return new ShareInfoCacheItem
        {
            Id = dbItem.Id,
            PageId = dbItem.PageId,
            UserId = dbItem.UserId,
            Token = dbItem.Token,
            Permission = dbItem.Permission,
            GrantedBy = dbItem.GrantedBy
        };
    }

    public ShareInfo ToDbItem()
    {
        return new ShareInfo
        {
            Id = Id,
            PageId = PageId,
            UserId = UserId,
            Token = Token,
            Permission = Permission,
            GrantedBy = GrantedBy
        };
    }
}