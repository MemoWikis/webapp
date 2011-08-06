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

        public SampleData(RegisterUser registerUser, QuestionRepository questionRepository)
        {
            _registerUser = registerUser;
            _questionRepository = questionRepository;
        }

        public void CreateUsers()
        {
            var robert = new User();
            robert.EmailAddress = "robert@robert-m.de";
            robert.UserName = "Robert";
            SetUserPassword.Run("fooBar", robert);
            _registerUser.Run(robert);

            var jule = new User();
            jule.EmailAddress = "jule@robert-m.de";
            jule.UserName = "Jule";
            SetUserPassword.Run("fooBar", robert);
            _registerUser.Run(jule);
        }

        public void ImportQuestions(string xmlFile)
        {
            var importer = new Importer(System.IO.File.ReadAllText(xmlFile));

            foreach (var question in importer.Questions)
            {
                _questionRepository.Create(question);
            }

        }
    }
}
