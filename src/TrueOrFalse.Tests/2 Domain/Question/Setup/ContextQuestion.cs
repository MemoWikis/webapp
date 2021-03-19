using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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

        public User Creator { get { return _contextUser.All[0]; } }

        private User _learner;
        public User Learner { get { return _learner ?? _contextUser.All[1]; } }

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

        public ContextQuestion SetLearner(User learner) { _learner = learner; return this; }

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

        public ContextQuestion AddQuestions(int amount, User creator = null, bool withId = false, IList<Category> categoriesQuestions = null)
        {
            for (var i = 0; i < amount; i++)
                AddQuestion("Question" + i, "Solution" + i, i, withId, creator, categoriesQuestions);
            return this;
        }

        public ContextQuestion AddQuestion(
            string questionText = "defaultText",
            string solutionText = "defaultSolution",
            int id = 0,
            bool withId = false,
            User creator = null,
            IList<Category> categories = null,
            int correctnessProbability = 0)
        {
            var question = new Question();
            if (withId)
                question.Id = id;

            question.Text = questionText;
            question.Solution = solutionText;
            question.SolutionType = SolutionType.Text;
            question.SolutionMetadataJson = new SolutionMetadataText { IsCaseSensitive = true, IsExactInput = false }.Json;
            question.Creator = creator ?? _contextUser.All.First();
            question.CorrectnessProbability = correctnessProbability == 0 ? Rand.Next(1, 101) : correctnessProbability;

            if (categories != null)
                question.CategoriesIds = categories;

            All.Add(question);

            if (_persistQuestionsImmediately)
                _questionRepo.Create(question);

            return this;
        }

        public ContextQuestion AddAnswer(string answer)
        {
            Sl.Resolve<AnswerQuestion>().Run(All.Last().Id, answer, Learner.Id, Guid.NewGuid(), 1, -1);

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

        public ContextQuestion TotalQualityEntries(int totalQualityEntries) { All.Last().TotalQualityEntries = totalQualityEntries; return this; }
        public ContextQuestion TotalQualityAvg(int totalQualityAvg) { All.Last().TotalQualityAvg = totalQualityAvg; return this; }
        public ContextQuestion TotalValuationAvg(int totalValuationAvg) { All.Last().TotalRelevancePersonalAvg = totalValuationAvg; return this; }

        public ContextQuestion AddCategory(string categoryName)
        {
            _contextCategory.Add(categoryName);
            All.Last().CategoriesIds.Add(_contextCategory.All.Last());
            return this;
        }

        public ContextQuestion Persist()
        {
            foreach (var question in All)
                _questionRepo.Create(question);

            _questionRepo.Flush();

            return this;
        }

        public static void PutQuestionIntoMemoryCache(int answerProbability, int id)
        {
            ContextCategory.New(false).AddToEntityCache("Category name", CategoryType.Standard, null, true);
            var categories = EntityCache.GetAllCategories(); 

            var questions = New().AddQuestion("", "", id, true, null, categories, answerProbability).All;
           
            var categoryIds = new List<int> { 1 };

            EntityCache.AddOrUpdate(questions[0], categoryIds);

            //put into memory cache
        }

        public static void PutQuestionsIntoMemoryCache(int amount = 20)
        {


            ContextCategory.New(false).AddToEntityCache("Category name", CategoryType.Standard, null, true, 1);
            var categories = EntityCache.GetAllCategories();

            var questions = New().AddQuestions(amount, null, true, categories).All;

            var categoryIds = new List<int> { 1 };

            foreach (var question in questions)
                EntityCache.AddOrUpdate(question, categoryIds);
        }


        public static List<UserCacheItem> SetWuwi(int amountQuestion)
        {
            var contextUser = ContextUser.New();
            var users = contextUser.Add().All;
            var categoryList = ContextCategory.New().Add("Daniel").All;
            categoryList.First().Id = 1; 

            var userCacheItem = new UserCacheItem();
            userCacheItem.User = users.FirstOrDefault();
            userCacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>();
            userCacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>();

            var questions = New().AddQuestions(amountQuestion, users.FirstOrDefault(), true, categoryList).All;
            users.ForEach(u => Sl.UserRepo.Create(u));
            UserCache.AddOrUpdate(users.FirstOrDefault());

            PutQuestionValuationsIntoUserCache(questions, users);

            return UserCache.GetAllCacheItems();
        }

        private static void PutQuestionValuationsIntoUserCache(List<Question> questions, List<User> users)
        {
            var rand = new Random();
            for (int i = 0; i < questions.Count; i++)
            {
                var questionValuation = new QuestionValuationCacheItem();

                questionValuation.Id = i;
                questionValuation.Question = questions[i];

                if (i == 0)
                    questionValuation.IsInWishKnowledge = false;
                else
                    questionValuation.IsInWishKnowledge = rand.Next(-1, 2) != -1;

                questionValuation.User = users.FirstOrDefault();
                UserCache.AddOrUpdate(questionValuation);
            }
        }
    }
}
