public class UserProfileController(
    SessionUser _sessionUser,
    UserWritingRepo _userWritingRepo) : ApiBaseController
{
    public readonly record struct UpdateAboutMeRequest(string AboutMeText);

    public readonly record struct UpdateAboutMeResult(
        bool Success,
        string ErrorMessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public UpdateAboutMeResult UpdateAboutMe([FromBody] UpdateAboutMeRequest request)
    {
        _userWritingRepo.ApplyChangeAndUpdate(_sessionUser.UserId, user =>
        {
            user.AboutMeText = request.AboutMeText;
        });

        return new UpdateAboutMeResult(true, "");
    }
}