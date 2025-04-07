using Seedworks.Lib.Persistence;

public class ShareCacheItem : IPersistable
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public UserCacheItem? User { get; set; }
    public string Token { get; set; } = "";
    public SharePermission Permission { get; set; } = SharePermission.View;
    public int GrantedBy { get; set; }

    public static ShareCacheItem ToCacheItem(Share dbItem)
    {
        var user = dbItem.UserId != null
            ? EntityCache.GetUserByIdNullable(dbItem.UserId.Value)
            : null;
        return new ShareCacheItem
        {
            Id = dbItem.Id,
            PageId = dbItem.PageId,
            User = user,
            Token = dbItem.Token,
            Permission = dbItem.Permission,
            GrantedBy = dbItem.GrantedBy
        };
    }

    public Share ToDbItem()
    {
        return new Share
        {
            Id = Id,
            PageId = PageId,
            UserId = User?.Id,
            Token = Token,
            Permission = Permission,
            GrantedBy = GrantedBy
        };
    }
}