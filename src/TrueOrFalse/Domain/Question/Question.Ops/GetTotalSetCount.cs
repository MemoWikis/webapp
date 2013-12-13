using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;

namespace TrueOrFalse
{
    public class UpdateSetDataForQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly QuestionRepository _questionRepo;

        public UpdateSetDataForQuestion(
            ISession session, 
            QuestionRepository questionRepo)
        {
            _session = session;
            _questionRepo = questionRepo;
        }

        public void Run()
        {
            foreach (var question in _questionRepo.GetAll()){
                Run(question);
            }
        }

        public void Run(Question question)
        {
            var questionsInSet = _session.QueryOver<QuestionInSet>()
                .Where(s => s.Question.Id == question.Id)
                .List<QuestionInSet>();

            question.SetsAmount = questionsInSet.Count;
            question.SetsTop5Json = JsonConvert.SerializeObject(
                questionsInSet
                    .Where(x => x.Set != null)
                    .Take(5)
                    .Select(x => new SetMini{Id = x.Set.Id, Name = x.Set.Name}));

            _questionRepo.Update(question);
        }
    }
}
