using System.Collections.Generic;

namespace TrueOrFalse
{
    public class SampleData : IRegisterAsInstancePerLifetime
    {
        private readonly UserWritingRepo _userWritingRepo;

        public SampleData(UserWritingRepo userWritingRepo)
        {
            _userWritingRepo = userWritingRepo;
        }

        public List<User> CreateUsers()
        {
            var stefan = new User();
            stefan.EmailAddress = "noackstefan@googlemail.com";
            stefan.Name = "Stefan Noack";
            SetUserPassword.Run("fooBar", stefan);
            _userWritingRepo.Register(stefan);

            var robert = new User();
            robert.EmailAddress = "robert@robert-m.de";
            robert.Name = "Robert Mischke";
            SetUserPassword.Run("fooBar", robert);
            _userWritingRepo.Register(robert);

            var jule = new User();
            jule.EmailAddress = "jule@robert-m.de";
            jule.Name = "Jule";
            SetUserPassword.Run("fooBar", robert);
            _userWritingRepo.Register(jule);

            return new List<User> {stefan, robert, jule};
        }
    }
}
