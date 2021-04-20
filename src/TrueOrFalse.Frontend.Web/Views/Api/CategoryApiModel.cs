using System;

public class CategoryApiModel : BaseModel
{
    public bool Pin(int categoryId) => Pin(categoryId.ToString());
    public bool Pin(string categoryId)
    {
        if (_sessionUser.User == null)
            return false;

        CategoryInKnowledge.Pin(Convert.ToInt32(categoryId), _sessionUser.User);
        if (UserCache.GetItem(_sessionUser.UserId).IsFiltered)
        {
            UserEntityCache.DeleteCacheForUser();
            UserEntityCache.Init();
        }

        return true;
    }
}
