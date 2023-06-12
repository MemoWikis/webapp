public class BaseModel : BaseResolve
{
    public int UserId => SessionUserLegacy.UserId;
}