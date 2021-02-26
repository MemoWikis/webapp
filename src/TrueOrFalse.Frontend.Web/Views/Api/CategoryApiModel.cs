using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class CategoryApiModel: BaseModel
    {
        public bool Pin(int categoryId) => Pin(categoryId.ToString()); 
        public  bool Pin(string categoryId)
        {
            if (_sessionUser.User == null)
                return false;

            CategoryInKnowledge.Pin(Convert.ToInt32(categoryId), _sessionUser.User);
            if (UserCache.IsFiltered)
            {
                UserEntityCache.DeleteCacheForUser();
                UserEntityCache.Init();
            }
            
            return true;
        }
    }
}