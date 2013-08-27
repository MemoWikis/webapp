using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Tests
{
    public class ContextSet : IRegisterAsInstancePerLifetime
    {
        private readonly ContextQuestion _contextQuestion = ContextQuestion.New();
        private readonly ContextUser _contextUser = ContextUser.New();
        private readonly ContextCategory _contextCategory = ContextCategory.New();
        private readonly SetRepository _setRepository;

        public List<Set> All = new List<Set>();
        
        public ContextSet(SetRepository setRepository)
        {
            _contextUser.Add("Context Set").Persist();
            _setRepository = setRepository;
        }

        public static ContextSet New()
        {
            return BaseTest.Resolve<ContextSet>();
        }

        public ContextSet AddSet(string name, string text = "")
        {
            var set = new Set();
            set.Name = name;
            set.Text = text;
            set.Creator = _contextUser.All.First();
            All.Add(set);

            return this;
        }

        public ContextSet AddCategory(string name)
        {
            var category = _contextCategory.Add(name).Persist().All.Last();
            All.Last().Categories.Add(category);
            return this;
        }

        public ContextSet AddQuestion(string question, string solution)
        {
            var addedQuestion = _contextQuestion.AddQuestion(question, solution).All.Last();
            var set = All.Last();
            var newQuestionInSet = new QuestionInSet{
                Question = addedQuestion,
                Set = set
            };
            set.QuestionsInSet.Add(newQuestionInSet);
            return this;
        }

        public ContextSet Persist()
        {
            foreach (var set in All)
                _setRepository.Create(set);

            _setRepository.Flush();

            return this;
        }


    }
}
