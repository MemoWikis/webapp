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
    public UpdateAboutMeResult UpdateAboutMe([FromRoute] int id, [FromBody] UpdateAboutMeRequest request)
    {
        // Ensure user can only update their own profile
        if (_sessionUser.UserId != id)
        {
            return new UpdateAboutMeResult(false, "error.unauthorized");
        }

        try
        {
            _userWritingRepo.ApplyChangeAndUpdate(id, user =>
            {
                user.AboutMeText = request.AboutMeText;
            });

            return new UpdateAboutMeResult(true, "");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating AboutMeText for user {UserId}", id);
            return new UpdateAboutMeResult(false, "error.user.updateFailed");
        }
    }
}
