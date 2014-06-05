using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class MenuLeftModel : BaseResolve
{
    public IList<MenuModelCategoryItem> Categories = new List<MenuModelCategoryItem>();
    public int WishKnowledgeCount;
    public bool IsInstallationAdmin;
    public Menu Menu;

    public int UnreadMessageCount = 0;

    public MenuLeftModel()
    {
        var userSession = Resolve<SessionUser>();
        var sessionUiData= Resolve<SessionUiData>();

        Menu = sessionUiData.Menu;
        if (userSession.User != null)
        {
            IsInstallationAdmin = userSession.IsInstallationAdmin;

            WishKnowledgeCount = Resolve<GetWishQuestionCountCached>().Run(userSession.User.Id);
            UnreadMessageCount = Resolve<GetUnreadMessageCount>().Run(userSession.User.Id);
        }
    }

    public string Active(MenuEntry menuEntry)
    {
        return Menu.Active(menuEntry);
    }

    public bool IsActive(MenuEntry menuEntry)
    {
        return !string.IsNullOrEmpty(Menu.Active(menuEntry));
    }
}