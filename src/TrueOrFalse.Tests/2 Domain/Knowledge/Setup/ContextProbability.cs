using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextProbability : IRegisterAsInstancePerLifetime
    {
        public List<Probability> All = new List<Probability>();

        private readonly ProbabilityRepo _probabilityRepo;

        public User User;

        public ContextProbability(ProbabilityRepo probabilityRepo, ContextUser contextUser)
        {
            _probabilityRepo = probabilityRepo;
            User = contextUser.Add("Major Tom").Persist().All[0];
        }

        public static ContextProbability New()
        {
            return BaseTest.Resolve<ContextProbability>();
        }

        public ContextProbability Add(IList<Question> questions)
        {
            questions.ForEach(q => Add(q));
            return this;
        }

        public ContextProbability Add(Question question)
        {
            var probabilty = new Probability
            {
                AnswerCount = 10,
                DateTimeCalculated = DateTime.Now,
                Percentage = 0.3m,
                Question = question,
                User = User
            };

            All.Add(probabilty);

            return this;
        }

        public ContextProbability Persist()
        {
            foreach (var probability in All)
                _probabilityRepo.Create(probability);

            return this;
        }
    }
}
