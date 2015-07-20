using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace TrueOrFalse.Tests
{
    public class ContextQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ContextUser _contextUser = ContextUser.New();
        private readonly ContextCategory _contextCategory = ContextCategory.New();

        private readonly QuestionRepository _questionRepository;

        public List<Question> All = new List<Question>();
        public User Creator { get { return _contextUser.All[0]; }}
        public User Learner { get { return _contextUser.All[1]; } }

        private bool _persistQuestionsImmediately;

        public static ContextQuestion New()
        {
            return BaseTest.Resolve<ContextQuestion>();
        }

        public ContextQuestion(QuestionRepository questionRepository)
        {
            _contextUser.Add("Creator").Persist();
            _contextUser.Add("Learner").Persist();
            _questionRepository = questionRepository;
        }

        public ContextQuestion PersistImmediately()
        {
            _persistQuestionsImmediately = true;
            return this;
        }

        public ContextQuestion AddQuestions(int amount)
        {
            for (var i = 0; i < amount; i++)
                AddQuestion("Question" + i, "Solution" + 1);

            return this;
        }

        public ContextQuestion AddQuestion(string questionText = "defaultText", string solutionText = "defaultSolution")
        {
            var question = new Question();
            question.Text = questionText;
            question.Solution = solutionText;
            question.SolutionType = SolutionType.Text;
            question.SolutionMetadataJson = new SolutionMetadataText{IsCaseSensitive = true, IsExactInput = false}.Json;
            question.Creator = _contextUser.All.First();
            All.Add(question);

            if (_persistQuestionsImmediately)
                _questionRepository.Create(question);

            return this;
        }

        public ContextQuestion AddAnswers(int countCorrect, int countWrong, DateTime dateCreated)
        {
            var lastQuestion = All.Last();

            for (var i = 0; i < countCorrect; i++)
                Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution, Learner.Id, dateCreated);

            for (var i = 0; i < countWrong; i++)
                Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution + "möb", Learner.Id, dateCreated);

            return this;
        }

        public ContextQuestion SetProbability(int probability, User learner)
        {
            var lastQuestion = All.Last();

            var questionValutionRepo = Sl.Resolve<QuestionValuationRepo>();
            var valuation = questionValutionRepo.GetBy(lastQuestion.Id, learner.Id);
            valuation.CorrectnessProbability = probability;
            questionValutionRepo.Update(valuation);

            return this;
        }

        public ContextQuestion TotalQualityEntries(int totalQualityEntries){ All.Last().TotalQualityEntries = totalQualityEntries; return this;}
        public ContextQuestion TotalQualityAvg(int totalQualityAvg) { All.Last().TotalQualityAvg = totalQualityAvg; return this; }
        public ContextQuestion TotalValuationAvg(int totalValuationAvg) { All.Last().TotalRelevancePersonalAvg = totalValuationAvg; return this; }

        public ContextQuestion AddCategory(string categoryName)
        {
            _contextCategory.Add(categoryName);
            All.Last().Categories.Add(_contextCategory.All.Last());
            return this;
        }

        public ContextQuestion Persist()
        {
            foreach (var question in All)
                _questionRepository.Create(question);

            _questionRepository.Flush();

            return this;
        }
    }
}
