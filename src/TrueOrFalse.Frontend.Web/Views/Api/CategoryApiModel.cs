using System;

public class CategoryApiModel : BaseModel
{
    public bool Pin(int categoryId) => Pin(categoryId.ToString());
    public bool Pin(string categoryId)
    {
        if (SessionUser.User == null)
            return false;

        CategoryInKnowledge.Pin(Convert.ToInt32(categoryId), SessionUser.User);
        if (UserEntityCache.IsCacheAvailable(SessionUser.UserId))
        {
            UserEntityCache.DeleteCacheForUser();
            UserEntityCache.Init();
        }

        return true;
    }
}
