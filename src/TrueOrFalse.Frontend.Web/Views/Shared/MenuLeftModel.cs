using System.Collections.Generic;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class MenuLeftModel
{
    public IList<MenuModelCategoryItem> Categories = new List<MenuModelCategoryItem>();
    public int WishKnowledgeCount;

    public MenuLeftModel()
    {
        var userSession = Sl.Resolve<SessionUser>();
        if (userSession.User != null)
            WishKnowledgeCount = Sl.Resolve<GetWishKnowledgeCountCached>().Run(userSession.User.Id);
    }
    
}