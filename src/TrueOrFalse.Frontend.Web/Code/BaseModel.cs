public class BaseModel : BaseResolve
{
    public int UserId => SessionUserLegacy.UserId;
    public UserMenu UserMenu;
    public BaseModel()
    {
        var sessionUiData = Resolve<SessionUiData>();
        UserMenu = sessionUiData.UserMenu;
    }
}