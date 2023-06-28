﻿using System.Collections.Generic;

namespace TrueOrFalse
{
    public class SampleData : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepo _questionRepo;
        private readonly CategoryRepository _categoryRepository;
        private readonly Importer _importer;
        private readonly RegisterUser _registerUser;

        public SampleData(
            QuestionRepo questionRepo, 
            CategoryRepository categoryRepository, 
            Importer importer,
            RegisterUser registerUser)
        {
            _questionRepo = questionRepo;
            _categoryRepository = categoryRepository;
            _importer = importer;
            _registerUser = registerUser;
        }

        public List<User> CreateUsers()
        {
            var stefan = new User();
            stefan.EmailAddress = "noackstefan@googlemail.com";
            stefan.Name = "Stefan Noack";
            SetUserPassword.Run("fooBar", stefan);
            _registerUser.Run(stefan);

            var robert = new User();
            robert.EmailAddress = "robert@robert-m.de";
            robert.Name = "Robert Mischke";
            SetUserPassword.Run("fooBar", robert);
            _registerUser.Run(robert);

            var jule = new User();
            jule.EmailAddress = "jule@robert-m.de";
            jule.Name = "Jule";
            SetUserPassword.Run("fooBar", robert);
            _registerUser.Run(jule);

            return new List<User> {stefan, robert, jule};
        }
    }
}
