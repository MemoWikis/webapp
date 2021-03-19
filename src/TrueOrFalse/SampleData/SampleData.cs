using System.Collections.Generic;

namespace TrueOrFalse
{
    public class SampleData : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepo _questionRepo;
        private readonly CategoryRepository _categoryRepository;
        private readonly Importer _importer;

        public SampleData(
            QuestionRepo questionRepo, 
            CategoryRepository categoryRepository, 
            Importer importer)
        {
            _questionRepo = questionRepo;
            _categoryRepository = categoryRepository;
            _importer = importer;
        }

        public List<User> CreateUsers()
        {
            var stefan = new User();
            stefan.EmailAddress = "noackstefan@googlemail.com";
            stefan.Name = "Stefan Noack";
            SetUserPassword.Run("fooBar", stefan);
            RegisterUser.Run(stefan);

            var robert = new User();
            robert.EmailAddress = "robert@robert-m.de";
            robert.Name = "Robert Mischke";
            SetUserPassword.Run("fooBar", robert);
            RegisterUser.Run(robert);

            var jule = new User();
            jule.EmailAddress = "jule@robert-m.de";
            jule.Name = "Jule";
            SetUserPassword.Run("fooBar", robert);
            RegisterUser.Run(jule);

            return new List<User> {stefan, robert, jule};
        }

        public void Import(string xmlFile)
        {
            var importerResult = _importer.Run(System.IO.File.ReadAllText(xmlFile));

            foreach (var category in importerResult.Categories){
                _categoryRepository.Create(Sl.CategoryRepo.GetByIdEager(category.Id));
            }
            foreach (var question in importerResult.Questions)
            {
                _questionRepo.Create(question);
            }
        }
    }
}
