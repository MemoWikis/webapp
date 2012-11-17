using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class MenuLeftModel
{
    public IList<MenuModelCategoryItem> Categories = new List<MenuModelCategoryItem>();
    public int WishKnowledgeCount;
    public bool IsInstallationAdmin;

    public MenuLeftModel()
    {
        var userSession = Sl.Resolve<SessionUser>();
        if (userSession.User != null)
        {
            WishKnowledgeCount = Sl.Resolve<GetWishKnowledgeCountCached>().Run(userSession.User.Id);
            IsInstallationAdmin = userSession.User.IsInstallationAdmin;
        }
    }
    
}