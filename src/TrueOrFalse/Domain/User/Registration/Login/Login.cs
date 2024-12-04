using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Domain.User
{
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
        public bool UserLogin(LoginParam param)
        {
            if (_credentialsAreValid.Yes(param.EmailAddress, param.Password))
            {
                if (param.PersistentLogin)
                {
                    WritePersistentLoginToCookie.Run(_credentialsAreValid.User.Id,
                        _persistentLoginRepo,
                        _httpContextAccessor.HttpContext);
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
}
