using System.Collections.Generic;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class ContextUser : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;

        public List<User> AllUsers = new List<User>();

        public ContextUser(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public static ContextUser New()
        {
            return BaseTest.Resolve<ContextUser>();
        }

        public ContextUser Add(string userName)
        {
            AllUsers.Add(new User {Name = userName});
            return this;
        }

        public ContextUser Persist()
        {
            foreach (var usr in AllUsers)
                _userRepository.Create(usr);

            return this;
        }
    }
}