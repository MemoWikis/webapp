using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Registration;

namespace TrueOrFalse.Core
{
    public class SampleData : IRegisterAsInstancePerLifetime
    {
        private readonly RegisterUser _registerUser;
        private readonly QuestionRepository _questionRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly Importer _importer;

        public SampleData(RegisterUser registerUser, QuestionRepository questionRepository, CategoryRepository categoryRepository, Importer importer)
        {
            _registerUser = registerUser;
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _importer = importer;
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

        public void ImportCategories(string xmlFile)
        {
            _importer.Run(System.IO.File.ReadAllText(xmlFile));

            foreach (var category in _importer.Categories)
            {
                _categoryRepository.Create(category);
            }

        }

        public void ImportQuestions(string xmlFile)
        {
             _importer.Run(System.IO.File.ReadAllText(xmlFile));

            foreach (var question in _importer.Questions)
            {
                _questionRepository.Create(question);
            }

        }
    }
}
