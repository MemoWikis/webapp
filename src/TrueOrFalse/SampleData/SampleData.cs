using System.Collections.Generic;
using TrueOrFalse.Registration;

namespace TrueOrFalse
{
    public class SampleData : IRegisterAsInstancePerLifetime
    {
        private readonly RegisterUser _registerUser;
        private readonly QuestionRepository _questionRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly Importer _importer;

        public SampleData(
            RegisterUser registerUser, 
            QuestionRepository questionRepository, 
            CategoryRepository categoryRepository, 
            Importer importer)
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

        public void Import(string xmlFile)
        {
            var importerResult = _importer.Run(System.IO.File.ReadAllText(xmlFile));

            foreach (var category in importerResult.Categories){
                _categoryRepository.Create(category);
            }
            foreach (var question in importerResult.Questions)
            {
                _questionRepository.Create(question);
            }
        }
    }
}
