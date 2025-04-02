public class ShareInfoHelper
{
    public static string GenerateShareToken(int pageId, int grantedById)
    {
        // Generate a share Token and a ShareInfoCacheItem and add the share info to the shareInfo repository and the EntityCache.
        var token = Guid.NewGuid().ToString();
        var shareInfo = new ShareInfoCacheItem
        {
            PageId = pageId,
            Token = token,
            Permission = SharePermission.View,
            GrantedBy = grantedById
        };
        EntityCache.AddOrUpdate(shareInfo);

        return shareInfo.Token;
    }
}