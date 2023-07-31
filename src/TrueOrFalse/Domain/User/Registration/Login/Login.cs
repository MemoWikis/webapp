using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Domain.User
{
    public class Login : IRegisterAsInstancePerLifetime
    {
        private readonly CredentialsAreValid _credentialsAreValid;
        private readonly UserWritingRepo _userWritingRepo;
        private readonly SessionUser _sessionUser;
        private readonly ActivityPointsRepo _activityPointsRepo;
        private readonly PersistentLoginRepo _persistentLoginRepo;

        public Login(CredentialsAreValid credentialsAreValid,
            UserWritingRepo userWritingRepo,
            SessionUser sessionUser,
            ActivityPointsRepo activityPointsRepo,
            PersistentLoginRepo persistentLoginRepo)
        {
            _credentialsAreValid = credentialsAreValid;
            _userWritingRepo = userWritingRepo;
            _sessionUser = sessionUser;
            _activityPointsRepo = activityPointsRepo;
            _persistentLoginRepo = persistentLoginRepo;
        }
        public bool UserLogin(LoginJson loginJson)
        {
            if (_credentialsAreValid.Yes(loginJson.EmailAddress, loginJson.Password))
            {

                if (loginJson.PersistentLogin)
                {
                    WritePersistentLoginToCookie.Run(_credentialsAreValid.User.Id, _persistentLoginRepo);
                }

                _sessionUser.Login(_credentialsAreValid.User);

                TransferActivityPoints.FromSessionToUser(_sessionUser, _activityPointsRepo);
                _userWritingRepo.UpdateActivityPointsData();
                return true;

            }

            return false;
        }
    }
}
