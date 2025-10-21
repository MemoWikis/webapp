public class ContextQuestion
{
    private readonly ContextUser _contextUser;
    private readonly AnswerRepo _answerRepo;
    private readonly AnswerQuestion _answerQuestion;
    private readonly PageRepository _pageRepository;
    private readonly QuestionWritingRepo _questionWritingRepo;

    public List<Question> All = new();
    public List<Answer> AllAnswers = new();


    private bool _persistQuestionsImmediately;
    private readonly Random _rand = new();

    public ContextQuestion(TestHarness testHarness, bool persistImmediately = false)
    {
        _persistQuestionsImmediately = persistImmediately;

        _contextUser = ContextUser.New(testHarness.R<UserWritingRepo>());
        _contextUser.Add("Creator").Persist();
        _contextUser.Add("Learner").Persist();
        _answerRepo = testHarness.R<AnswerRepo>();
        _answerQuestion = testHarness.R<AnswerQuestion>();
        _pageRepository = testHarness.R<PageRepository>();
        _questionWritingRepo = testHarness.R<QuestionWritingRepo>();
    }

    public User Creator => _contextUser.All[0];
    public User Learner => _contextUser.All[1];

    public ContextQuestion AddAnswer(string answer)
    {
        _answerQuestion.Run(All.Last().Id, answer, Learner.Id, Guid.NewGuid(), 1, -1);
        _answerRepo.Flush();

        AllAnswers.Add(_answerRepo.GetLastCreated());

        return this;
    }

    public ContextQuestion AddQuestion(
        string questionText = "defaultText",
        string solutionText = "defaultSolution",
        int id = 0,
        bool withId = false,
        User? creator = null,
        IList<Page> pages = null,
        int correctnessProbability = 0,
        bool persistImmediately = false,
        QuestionVisibility questionVisibility = QuestionVisibility.Private)

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
        question.CorrectnessProbability = correctnessProbability == 0 ? _rand.Next(1, 101) : correctnessProbability;
        question.Visibility = questionVisibility;
        if (pages != null)
        {
            question.Pages = pages;
        }

        All.Add(question);

        if (_persistQuestionsImmediately || persistImmediately)
        {
            _questionWritingRepo.Create(question);
        }

        return this;
    }

    public ContextQuestion AddRandomQuestions(
        int amount,
        User creator = null,
        bool withId = false,
        IList<Page> pagesQuestions = null,
        bool persistImmediately = false)
    {
        for (var i = 0; i < amount; i++)
        {
            AddQuestion("Question" + i, "Solution" + i, i, withId, creator, pagesQuestions,
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

    public ContextQuestion Persist()
    {
        foreach (var question in All)
        {
            _questionWritingRepo.Create(question);
        }

        _questionWritingRepo.Flush();

        return this;
    }
}