using Microsoft.AspNetCore.Http;

public class Login(
    CredentialsAreValid _credentialsAreValid,
    UserWritingRepo _userWritingRepo,
    SessionUser _sessionUser,
    ActivityPointsRepo _activityPointsRepo,
    PersistentLoginRepo _persistentLoginRepo,
    IHttpContextAccessor _httpContextAccessor,
    PageViewRepo _pageViewRepo)
    : IRegisterAsInstancePerLifetime
{
    public bool UserLogin(LoginRequest request)
    {
        if (_credentialsAreValid.Yes(request.EmailAddress, request.Password))
        {
            if (request.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(_credentialsAreValid.User.Id,
                    _persistentLoginRepo,
                    _httpContextAccessor.HttpContext
                );
            }

            var user = _credentialsAreValid.User;
            _sessionUser.Login(user, _pageViewRepo);

            TransferActivityPoints.FromSessionToUser(_sessionUser, _activityPointsRepo);
            _userWritingRepo.UpdateActivityPointsData();
            _userWritingRepo.Update(user);
            return true;
        }

        return false;
    }
}