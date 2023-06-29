using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NHibernate;

namespace TrueOrFalse.Tests;

public class ContextQuestion
{
    private readonly ContextUser _contextUser = ContextUser.New();
    private readonly ContextCategory _contextCategory = ContextCategory.New();

    private readonly QuestionRepo _questionRepo;

    public List<Question> All = new();
    public List<Answer> AllAnswers = new();

    private User _learner;

    private bool _persistQuestionsImmediately;
    private readonly Random Rand = new();

    private ContextQuestion()
    {
        _contextUser.Add("Creator").Persist();
        _contextUser.Add("Learner").Persist();
        _questionRepo = Sl.R<QuestionRepo>();
    }

    public User Creator => _contextUser.All[0];
    public User Learner => _learner ?? _contextUser.All[1];

    public ContextQuestion AddAnswer(string answer)
    {
        Sl.Resolve<AnswerQuestion>().Run(All.Last().Id, answer, Learner.Id, Guid.NewGuid(), 1, -1);

        var answerRepo = Sl.R<AnswerRepo>();
        answerRepo.Flush();

        AllAnswers.Add(answerRepo.GetLastCreated());

        return this;
    }

    public ContextQuestion AddAnswers(int countCorrect, int countWrong, DateTime dateCreated = default)
    {
        if (dateCreated == default)
        {
            dateCreated = DateTime.Now;
        }

        var lastQuestion = All.Last();

        for (var i = 0; i < countCorrect; i++)
        {
            Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution, Learner.Id, Guid.NewGuid(), 1, -1,
                dateCreated);
        }

        for (var i = 0; i < countWrong; i++)
        {
            Sl.Resolve<AnswerQuestion>().Run(lastQuestion.Id, lastQuestion.Solution + "möb", Learner.Id, Guid.NewGuid(),
                1, -1, dateCreated);
        }

        return this;
    }

    public ContextQuestion AddCategory(string categoryName, EntityCacheInitializer entityCacheInitializer)
    {
        _contextCategory.Add(categoryName).Persist();
        entityCacheInitializer.Init();
        All.Last().Categories.Add(_contextCategory.All.Last());
        return this;
    }

    public ContextQuestion AddQuestion(
        string questionText = "defaultText",
        string solutionText = "defaultSolution",
        int id = 0,
        bool withId = false,
        User creator = null,
        IList<Category> categories = null,
        int correctnessProbability = 0,
        bool persistImmediately = false)

    {
        var question = new Question();
        if (withId)
        {
            question.Id = id;
        }

        question.Text = questionText;
        question.Solution = solutionText;
        question.SolutionType = SolutionType.Text;
        question.SolutionMetadataJson = new SolutionMetadataText { IsCaseSensitive = true, IsExactInput = false }.Json;
        question.Creator = creator ?? _contextUser.All.First();
        question.CorrectnessProbability = correctnessProbability == 0 ? Rand.Next(1, 101) : correctnessProbability;

        if (categories != null)
        {
            question.Categories = categories;
        }

        All.Add(question);

        if (_persistQuestionsImmediately || persistImmediately)
        {
            _questionRepo.Create(question);
        }

        return this;
    }

    public ContextQuestion AddQuestions(
        int amount,
        User creator = null,
        bool withId = false,
        IList<Category> categoriesQuestions = null,
        bool persistImmediately = false)
    {
        for (var i = 0; i < amount; i++)
        {
            AddQuestion("Question" + i, "Solution" + i, i, withId, creator, categoriesQuestions,
                persistImmediately: persistImmediately);
        }

        return this;
    }

    public ContextQuestion AddToWishknowledge(User user, QuestionInKnowledge questionInKnowledge)
    {
        var lastQuestion = All.Last();
        questionInKnowledge.Pin(lastQuestion.Id, user.Id);

        return this;
    }

    public static Question GetQuestion()
    {
        return New().AddQuestion().Persist().All[0];
    }

    public static ContextQuestion New(bool persistImmediately = false)
    {
        var result = new ContextQuestion();

        if (persistImmediately)
        {
            result.PersistImmediately();
        }

        return result;
    }

    public ContextQuestion Persist()
    {
        foreach (var question in All)
        {
            _questionRepo.Create(question);
        }

        _questionRepo.Flush();

        return this;
    }

    public ContextQuestion PersistImmediately()
    {
        _persistQuestionsImmediately = true;
        return this;
    }

    public static void PutQuestionIntoMemoryCache(int answerProbability, int id, CategoryRepository categoryRepository)
    {
        ContextCategory.New(false).AddToEntityCache("Category name").Persist();
        var categories = categoryRepository.GetAllEager();

        var questions = New().AddQuestion("", "", id, true, null, categories, answerProbability).All;

        var categoryIds = new List<int> { 1 };

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(questions[0]), categoryIds);
    }

    public static void PutQuestionsIntoMemoryCache(CategoryRepository categoryRepository, int amount = 20)
    {
        ContextCategory.New(false).AddToEntityCache("Category name").Persist();
        var categories = categoryRepository.GetAllEager();

        var questions = New().AddQuestions(amount, null, true, categories).All;

        var categoryIds = new List<int> { 1 };

        foreach (var question in questions)
        {
            EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question), categoryIds);
        }
    }

    public static List<SessionUserCacheItem> SetWuwi(int amountQuestion, CategoryValuationRepo categoryValuationRepo)
    {
        var contextUser = ContextUser.New();
        var users = contextUser.Add().All;
        var categoryList = ContextCategory.New().Add("Daniel").All;
        categoryList.First().Id = 1;

        var questions = New().AddQuestions(amountQuestion, users.FirstOrDefault(), true, categoryList).All;
        users.ForEach(u => Sl.UserRepo.Create(u));
        SessionUserCache.AddOrUpdate(users.FirstOrDefault(),categoryValuationRepo);

        PutQuestionValuationsIntoUserCache(questions, users, categoryValuationRepo);

        return SessionUserCache.GetAllCacheItems(categoryValuationRepo);
    }

    private static void PutQuestionValuationsIntoUserCache(List<Question> questions, List<User> users, CategoryValuationRepo categoryValuationRepo)
    {
        var rand = new Random();
        for (var i = 0; i < questions.Count; i++)
        {
            var questionValuation = new QuestionValuationCacheItem();

            questionValuation.Id = i;
            questionValuation.Question = QuestionCacheItem.ToCacheQuestion(questions[i]);

            if (i == 0)
            {
                questionValuation.IsInWishKnowledge = false;
            }
            else
            {
                questionValuation.IsInWishKnowledge = rand.Next(-1, 2) != -1;
            }

            questionValuation.User = SessionUserCache.CreateItemFromDatabase(users.FirstOrDefault().Id, categoryValuationRepo);
            SessionUserCache.AddOrUpdate(questionValuation, categoryValuationRepo);
        }
    }
}