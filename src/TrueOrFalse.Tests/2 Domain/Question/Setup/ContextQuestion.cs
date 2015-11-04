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

        private readonly QuestionRepo _questionRepo;

        public List<Question> All = new List<Question>();
        public User Creator { get { return _contextUser.All[0]; }}

        private User _learner;
        public User Learner{get{ return _learner ??_contextUser.All[1];}}

        private bool _persistQuestionsImmediately;

        public static ContextQuestion New()
        {
            return BaseTest.Resolve<ContextQuestion>();
        }

        public static Question GetQuestion()
        {
            return New().AddQuestion().Persist().All[0];
        }

        public ContextQuestion SetLearner(User learner){ _learner = learner; return this; }

        public ContextQuestion(QuestionRepo questionRepo)
        {
            _contextUser.Add("Creator").Persist();
            _contextUser.Add("Learner").Persist();
            _questionRepo = questionRepo;
        }

        public ContextQuestion PersistImmediately()
        {
            _persistQuestionsImmediately = true;
            return this;
        }

        public ContextQuestion AddQuestions(int amount)
        {
            for (var i = 0; i < amount; i++)
                AddQuestion(questionText: "Question" + i, solutionText: "Solution" + 1);

            return this;
        }

        public ContextQuestion AddQuestion(string questionText = "defaultText", string solutionText = "defaultSolution", User creator = null)
        {
            var question = new Question();
            question.Text = questionText;
            question.Solution = solutionText;
            question.SolutionType = SolutionType.Text;
            question.SolutionMetadataJson = new SolutionMetadataText{IsCaseSensitive = true, IsExactInput = false}.Json;
            question.Creator = creator ?? _contextUser.All.First();

            All.Add(question);

            if (_persistQuestionsImmediately)
                _questionRepo.Create(question);

            return this;
        }

        public ContextQuestion AddAnswers(int countCorrect, int countWrong, DateTime dateCreated = default(DateTime))
        {
            if (dateCreated == default(DateTime))
                dateCreated = DateTime.Now;

            var lastQuestion = All.Last();

            for (var i = 0; i < countCorrect; i++)
                Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution, Learner.Id, dateCreated);

            for (var i = 0; i < countWrong; i++)
                Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution + "möb", Learner.Id, dateCreated);

            return this;
        }

        public ContextQuestion AddToWishknowledge(User user)
        {
            var lastQuestion = All.Last();
            
            Sl.R<QuestionValuationRepo>().Create(
                new QuestionValuation { Question = lastQuestion, User = user, RelevancePersonal = 50 }
            );

            return this;
        }

        public ContextQuestion SetProbability(int probability, User learner)
        {
            var lastQuestion = All.Last();

            var questionValutionRepo = Sl.Resolve<QuestionValuationRepo>();

            var valuation = questionValutionRepo.GetBy(lastQuestion.Id, learner.Id);

            if (valuation == null)
            {
                Sl.Resolve<ProbabilityUpdate_Valuation>().Run(lastQuestion.Id, learner.Id);
                valuation = questionValutionRepo.GetBy(lastQuestion.Id, learner.Id);
            }
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
                _questionRepo.Create(question);

            _questionRepo.Flush();

            return this;
        }
    }
}
