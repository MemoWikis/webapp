
namespace FacebookHelper;
public class LoginJson
{
    public string facebookUserId { get; set; }
    public string facebookAccessToken { get; set; }
}
public class CreateAndLoginJson
{
    public FacebookUserCreateParameter facebookUser { get; set; }
    public string facebookAccessToken { get; set; }
}