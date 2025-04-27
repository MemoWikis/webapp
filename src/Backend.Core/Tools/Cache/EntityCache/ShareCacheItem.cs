public class ShareCacheItem : IPersistable
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public UserCacheItem? SharedWith { get; set; } = null;
    public string Token { get; set; } = "";
    public SharePermission Permission { get; set; } = SharePermission.View;
    public int GrantedBy { get; set; }

    public static ShareCacheItem ToCacheItem(Share dbItem)
    {
        var user = dbItem.User != null
            ? EntityCache.GetUserByIdNullable(dbItem.User.Id)
            : null;

        return new ShareCacheItem
        {
            Id = dbItem.Id,
            PageId = dbItem.PageId,
            SharedWith = user,
            Token = dbItem.Token,
            Permission = dbItem.Permission,
            GrantedBy = dbItem.GrantedBy
        };
    }
}