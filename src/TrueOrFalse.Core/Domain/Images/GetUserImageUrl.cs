using TrueOrFalse.Core;

public class GetUserImageUrl : GetImageUrl
    {
    public string Run(User user)
    {
        return Run(user.Id);
    }

    protected override string PlaceholderImage
    {
        get { return "/Images/no-profile-picture-{0}.png"; }
    }

    protected override string RelativePath
    {
        get {  return "/Images/Users/"; }
    }
}