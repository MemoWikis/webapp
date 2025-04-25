using static LimitCheck;



public class UserMembershipPlansController : ApiBaseController
{
    [HttpGet]
    public BasicLimits GetBasicLimits()
    {
        var limits = LimitCheck.GetBasicLimits();
        return limits;
    }
}