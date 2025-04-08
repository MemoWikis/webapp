using Seedworks.Lib.Persistence;

public class ShareCacheItem : IPersistable
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public UserCacheItem? SharedWith { get; set; }
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

    public Share ToDbItem(UserReadingRepo userReadingRepo)
    {
        User? user = null;
        if (SharedWith != null)
            user = userReadingRepo.GetById(SharedWith.Id);

        return new Share
        {
            Id = Id,
            PageId = PageId,
            User = user,
            Token = Token,
            Permission = Permission,
            GrantedBy = GrantedBy
        };
    }
}