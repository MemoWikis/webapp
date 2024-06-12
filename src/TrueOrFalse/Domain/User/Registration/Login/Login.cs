﻿using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Domain.User
{
    public class Login : IRegisterAsInstancePerLifetime
    {
        private readonly CredentialsAreValid _credentialsAreValid;
        private readonly UserWritingRepo _userWritingRepo;
        private readonly SessionUser _sessionUser;
        private readonly ActivityPointsRepo _activityPointsRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Login(CredentialsAreValid credentialsAreValid,
            UserWritingRepo userWritingRepo,
            SessionUser sessionUser,
            ActivityPointsRepo activityPointsRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _credentialsAreValid = credentialsAreValid;
            _userWritingRepo = userWritingRepo;
            _sessionUser = sessionUser;
            _activityPointsRepo = activityPointsRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool UserLogin(LoginParam param)
        {
            if (_credentialsAreValid.Yes(param.EmailAddress, param.Password))
            {
                if (param.PersistentLogin)
                {
                    WritePersistentLoginToCookie.Run(_credentialsAreValid.User.Id,
                        _httpContextAccessor);
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
