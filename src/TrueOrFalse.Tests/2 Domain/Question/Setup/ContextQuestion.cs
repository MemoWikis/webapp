using System;
using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextQuestion
    {
        private readonly ContextUser _contextUser = ContextUser.New();
        private readonly ContextCategory _contextCategory = ContextCategory.New();

        private readonly QuestionRepo _questionRepo;

        public List<Question> All = new List<Question>();
        public List<Answer> AllAnswers = new List<Answer>();

        public User Creator { get { return _contextUser.All[0]; }}

        private User _learner;
        public User Learner{get{ return _learner ??_contextUser.All[1];}}

        private bool _persistQuestionsImmediately;
        private Random Rand = new Random();

        public static ContextQuestion New(bool persistImmediately = false)
        {
            var result = new ContextQuestion();

            if (persistImmediately)
                result.PersistImmediately();

            return result;
        }

        public static Question GetQuestion()
        {
            return New().AddQuestion().Persist().All[0];
        }

        public ContextQuestion SetLearner(User learner){ _learner = learner; return this; }

        private ContextQuestion()
        {
            _contextUser.Add("Creator").Persist();
            _contextUser.Add("Learner").Persist();
            _questionRepo = Sl.R<QuestionRepo>();
        }

        public ContextQuestion PersistImmediately()
        {
            _persistQuestionsImmediately = true;
            return this;
        }

        public ContextQuestion AddQuestions(int amount, User creator = null, bool withId = false)
        {
            for (var i = 0; i < amount; i++)
                AddQuestion(questionText: "Question" + i, solutionText: "Solution" + i, i , withId,  creator: creator);


            return this;
        }

        public ContextQuestion AddQuestion(
            string questionText = "defaultText", 
            string solutionText = "defaultSolution", 
            int id = 0,
            bool withId= false,
            User creator = null, 
            IList<Category> categories = null)
        {
            var question = new Question();
            if (withId)
                question.Id = id; 

            question.Text = questionText;
            question.Solution = solutionText;
            question.SolutionType = SolutionType.Text;
            question.SolutionMetadataJson = new SolutionMetadataText{IsCaseSensitive = true, IsExactInput = false}.Json;
            question.Creator = creator ?? _contextUser.All.First();
            question.CorrectnessProbability = Rand.Next(1, 101); 
            question.Categories = new List<Category> { ContextCategory.New().Add("blabla",CategoryType.Standard,null,true).All[0]
            };

            if (categories != null)
                question.Categories = categories;

            All.Add(question);

            if (_persistQuestionsImmediately)
                _questionRepo.Create(question);

            return this;
        }

        public ContextQuestion AddAnswer(string answer)
        {
            var result = Sl.Resolve<AnswerQuestion>().Run(All.Last().Id, answer, Learner.Id, Guid.NewGuid(), 1, -1);

            var answerRepo = Sl.R<AnswerRepo>();
            answerRepo.Flush();

            AllAnswers.Add(answerRepo.GetLastCreated());

            return this;
            
        }

        public ContextQuestion AddAnswers(int countCorrect, int countWrong, DateTime dateCreated = default(DateTime))
        {
            if (dateCreated == default(DateTime))
                dateCreated = DateTime.Now;

            var lastQuestion = All.Last();

            for (var i = 0; i < countCorrect; i++)
                Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution, Learner.Id, Guid.NewGuid(), 1, -1, dateCreated);

            for (var i = 0; i < countWrong; i++)
                Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution + "möb", Learner.Id, Guid.NewGuid(), 1, -1, dateCreated);

            return this;
        }

        public ContextQuestion AddToWishknowledge(User user)
        {
            var lastQuestion = All.Last();
            
            QuestionInKnowledge.Pin(lastQuestion.Id, user);

            return this;
        }

        public ContextQuestion SetProbability(int probability, User learner)
        {
            var lastQuestion = All.Last();

            var questionValutionRepo = Sl.Resolve<QuestionValuationRepo>();

            var valuation = questionValutionRepo.GetBy(lastQuestion.Id, learner.Id);

            if (valuation == null)
            {
                ProbabilityUpdate_Valuation.Run(lastQuestion.Id, learner.Id);
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

        public static List<Question> GetAllQuestionFromMemoryCache()
        {
            var questions = New().AddQuestions(5000, null,true).All;

            ContextCategory.New().AddToEntityCache("blabla", CategoryType.Standard,null,true);


            var categoryIds = new List<int>{0};
            foreach (var  question in questions) 
                EntityCache.AddOrUpdate(question, categoryIds);

            return EntityCache.GetQuestionsForCategory(0).ToList();

        }
    }
}
