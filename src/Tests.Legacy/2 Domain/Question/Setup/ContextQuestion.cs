using System;
using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests;

public class ContextQuestion
{
    private readonly ContextUser _contextUser;
    private readonly ContextCategory _contextCategory = ContextCategory.New();

    private readonly AnswerRepo _answerRepo;
    private readonly AnswerQuestion _answerQuestion;
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionWritingRepo _questionWritingRepo;

    public List<Question> All = new();
    public List<Answer> AllAnswers = new();

    private User _learner;

    private bool _persistQuestionsImmediately;
    private readonly Random Rand = new();

    private ContextQuestion(QuestionWritingRepo questionWritingRepo,
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion,
        UserWritingRepo userWritingRepo,
        CategoryRepository categoryRepository)
    {
        _contextUser = ContextUser.New(userWritingRepo);
        _contextUser.Add("Creator").Persist();
        _contextUser.Add("Learner").Persist();
        _answerRepo = answerRepo;
        _answerQuestion = answerQuestion;
        _categoryRepository = categoryRepository;
        _questionWritingRepo = questionWritingRepo;
    }

    public User Creator => _contextUser.All[0];
    public User Learner => _learner ?? _contextUser.All[1];

    public ContextQuestion AddAnswer(string answer)
    {
        _answerQuestion.Run(All.Last().Id, answer, Learner.Id, Guid.NewGuid(), 1, -1);
        _answerRepo.Flush();

        AllAnswers.Add(_answerRepo.GetLastCreated());

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
            _questionWritingRepo.Create(question, _categoryRepository);
        }

        return this;
    }

    public ContextQuestion AddRandomQuestions(
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

    public ContextQuestion AddToWishKnowledge(User user, QuestionInKnowledge questionInKnowledge)
    {
        var lastQuestion = All.Last();
        questionInKnowledge.Pin(lastQuestion.Id, user.Id);

        return this;
    }

    public static Question GetQuestion(
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion,
        UserWritingRepo userWritingRepo,
        CategoryRepository categoryRepository,
        QuestionWritingRepo questionWritingRepo)
    {
        return New(questionWritingRepo, answerRepo, answerQuestion, userWritingRepo, categoryRepository).AddQuestion().Persist().All[0];
    }

    public static ContextQuestion New(QuestionWritingRepo questionWritingRepo,
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion,
        UserWritingRepo userWritingRepo,
        CategoryRepository categoryRepository, 
        bool persistImmediately = false)
    {
        var result = new ContextQuestion(questionWritingRepo, answerRepo, answerQuestion, userWritingRepo, categoryRepository);

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
            _questionWritingRepo.Create(question);
        }

        _questionWritingRepo.Flush();

        return this;
    }

    public ContextQuestion PersistImmediately()
    {
        _persistQuestionsImmediately = true;
        return this;
    }

    public static void PutQuestionsIntoMemoryCache(CategoryRepository categoryRepository,
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion,
        UserWritingRepo userWritingRepo,
        QuestionWritingRepo questionWritingRepo,
        int amount = 20)
    {
        ContextCategory.New(false).AddToEntityCache("Category name").Persist();
        var categories = categoryRepository.GetAllEager();

        var questions = New(questionWritingRepo, answerRepo, answerQuestion, userWritingRepo, categoryRepository)
            .AddRandomQuestions(amount, null, true, categories).All;

        var categoryIds = new List<int> { 1 };

        foreach (var question in questions)
        {
            EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question), categoryIds);
        }
    }

    public static List<SessionUserCacheItem> SetWishKnowledge(int amountQuestion,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationRepo,
        CategoryRepository categoryRepository,
        QuestionWritingRepo questionWritingRepo,
        UserWritingRepo userWritingRepo)
    {
        var contextUser = ContextUser.New(userWritingRepo);
        var users = contextUser.Add().All;
        var categoryList = ContextCategory.New().Add("Daniel").All;
        categoryList.First().Id = 1;

        var questions = New(questionWritingRepo, answerRepo, answerQuestion, userWritingRepo, categoryRepository)
            .AddRandomQuestions(amountQuestion, users.FirstOrDefault(), true, categoryList).All;
        users.ForEach(u => userWritingRepo.Create(u));

        //SessionUserCache.AddOrUpdate(users.FirstOrDefault());

        PutQuestionValuationsIntoUserCache(questions, users, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

        //return SessionUserCache.GetAllCacheItems();

        throw new NotImplementedException();
    }

    private static void PutQuestionValuationsIntoUserCache(List<Question> questions, List<User> users, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationReadingRepo questionValuationRepo)
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

            //questionValuation.User = SessionUserCache.CreateItemFromDatabase(users.FirstOrDefault().Id, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);
            //SessionUserCache.AddOrUpdate(questionValuation, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

            throw new NotImplementedException();
        }
    }
}