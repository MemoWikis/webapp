using Serilog;

/// <summary>
/// Builds a deterministic test scenario. All _random decisions are based on the seed
/// defined in <see cref="ScenarioConfiguration"/>.
/// </summary>
public sealed class ScenarioBuilder
{
    private readonly TestHarness _testHarness;
    private readonly ScenarioConfiguration _configuration;
    private readonly PerformanceLogger _performanceLogger;
    private readonly ContentLibrary _contentLibrary;

    private readonly Random _random;
    private readonly List<User> _users = new();
    private readonly List<Page> _pages = new();
    private readonly List<Question> _questions = new();

    private readonly Dictionary<int, List<Page>> _pagesPerUser = new();
    private readonly Dictionary<int, List<Question>> _questionsPerUser = new();
    private readonly Dictionary<int, WikiContentDefinition> _wikiContent = new();

    public ScenarioBuilder(TestHarness testHarness, ScenarioConfiguration configuration, PerformanceLogger performanceLogger)
    {
        _testHarness = testHarness;
        _performanceLogger = performanceLogger;
        _configuration = configuration;
        _contentLibrary = new ContentLibrary();

        _random = new Random(_configuration.SeedForRandom);
    }

    public async Task BuildAsync()
    {
        _performanceLogger.Log("Start BuildAsync");

        CreateUsersAsync();
        _performanceLogger.Log("Users created");

        await CreatePagesAsync();
        _performanceLogger.Log("Pages created");

        await CreateQuestionsAsync();
        _performanceLogger.Log("Questions created");

        await GenerateLearningHistoryAsync(_users[0].Id);
        _performanceLogger.Log("Learning history created");
    }


    private void CreateUsersAsync()
    {
        var userWritingRepository = _testHarness.R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepository);

        int monthsAgoCounter = 1;
        foreach (var userDef in _configuration.DefaultUsers)
        {
            var user = new User
            {
                Name = userDef.GetDisplayName(),
                EmailAddress = userDef.EmailAddress,
                IsEmailConfirmed = true,
                IsInstallationAdmin = userDef.IsAdministrator,
                DateCreated = _configuration.Now.AddMonths(-monthsAgoCounter++)
            };

            SetUserPassword.Run("test", user);
            contextUser.Add(user);
            contextUser.Persist();

            _users.Add(user);
            _pagesPerUser[user.Id] = [];
            _questionsPerUser[user.Id] = [];
        }
    }

    private async Task CreatePagesAsync()
    {
        var contextPage = new ContextPage(_testHarness, false);

        for (int userIndex = 0; userIndex < _users.Count; userIndex++)
        {
            var user = _users[userIndex];
            var userDef = _configuration.DefaultUsers[userIndex];

            // Load wikis from content library
            var wikis = _contentLibrary.GetRandomWikisForTheme(
                userDef.ThemeFocus,
                _configuration.WikisPerUser,
                _random);

            // Fallback to generated pages if no content available
            if (wikis.Count == 0)
            {
                Log.Warning("No wiki content found for theme: {Theme}. Using placeholder pages.", userDef.ThemeFocus);
                await CreatePlaceholderPagesAsync(user, contextPage, userDef.ThemeFocus);
            }
            else
            {
                await CreateWikisFromContentAsync(user, contextPage, wikis);
            }
        }

        _pages.AddRange(contextPage.All);
    }

    private async Task CreateWikisFromContentAsync(
        User user,
        ContextPage contextPage,
        List<WikiContentDefinition> wikis)
    {
        foreach (var wiki in wikis)
        {
            // Create main wiki page
            contextPage
                .Add(wiki.MainPage.Title, user, isWiki: true)
                .Persist();

            var wikiPage = contextPage.All.Last();
            wikiPage.Content = wiki.MainPage.Content;
            _pagesPerUser[user.Id].Add(wikiPage);
            _wikiContent[wikiPage.Id] = wiki;

            // Create subpages
            foreach (var subpage in wiki.Subpages)
            {
                contextPage
                    .Add(subpage.Title, user, isWiki: false)
                    .Persist();

                var childPage = contextPage.All.Last();
                childPage.Content = subpage.Content;
                _pagesPerUser[user.Id].Add(childPage);

                // Add child relation (this persists directly)
                contextPage.AddChild(wikiPage, childPage);
            }
        }

        await Task.CompletedTask;
    }

    private async Task CreatePlaceholderPagesAsync(
        User user,
        ContextPage contextPage,
        string topicPrefix)
    {
        // Create 1-2 placeholder wikis with basic structure
        for (int pageIndex = 1; pageIndex <= 2; pageIndex++)
        {
            string pageName = $"{topicPrefix} Topic {pageIndex}";
            contextPage
                .Add(pageName, user, isWiki: true)
                .Persist();

            var rootPage = contextPage.All.Last();
            _pagesPerUser[user.Id].Add(rootPage);

            // Create 2-3 subpages
            int childCount = _random.Next(2, 4);
            for (int childIndex = 1; childIndex <= childCount; childIndex++)
            {
                string childName = $"{topicPrefix} Subtopic {pageIndex}-{childIndex}";
                contextPage
                    .Add(childName, user, isWiki: false)
                    .Persist();

                var childPage = contextPage.All.Last();
                _pagesPerUser[user.Id].Add(childPage);

                // Add child relation (this persists directly)
                contextPage.AddChild(rootPage, childPage);
            }
        }

        await Task.CompletedTask;
    }



    private async Task CreateQuestionsAsync()
    {
        var questionWritingRepository = _testHarness.R<QuestionWritingRepo>();

        foreach (var user in _users)
        {
            foreach (var page in _pagesPerUser[user.Id])
            {
                // Try to find questions from content library
                var questionsFromContent = GetQuestionsForPage(page);

                if (questionsFromContent.Count > 0)
                {
                    // Use questions from JSON content
                    foreach (var questionDef in questionsFromContent)
                    {
                        var question = new Question
                        {
                            Text = questionDef.Text,
                            Solution = questionDef.Solution,
                            SolutionType = SolutionType.Text,
                            SolutionMetadataJson = new SolutionMetadataText
                            {
                                IsCaseSensitive = questionDef.IsCaseSensitive,
                                IsExactInput = false
                            }.Json,
                            Creator = user,
                            CorrectnessProbability = _random.Next(30, 91),
                            Visibility = QuestionVisibility.Public,
                            Pages = new List<Page> { page }
                        };

                        questionWritingRepository.Create(question);
                        _questions.Add(question);
                        _questionsPerUser[user.Id].Add(question);
                    }
                }
                else
                {
                    // Fallback: generate placeholder questions
                    int questionsPerPage = user.Name == "QuestionContributor"
                        ? _configuration.QuestionsPerPageForContributor
                        : _configuration.QuestionsPerPageForNormalUsers;

                    for (int questionIndex = 1; questionIndex <= questionsPerPage; questionIndex++)
                    {
                        var question = new Question
                        {
                            Text = $"Question {questionIndex} for page {page.Name}?",
                            Solution = $"Answer {questionIndex} for page {page.Name}",
                            SolutionType = SolutionType.Text,
                            SolutionMetadataJson = new SolutionMetadataText
                            {
                                IsCaseSensitive = false,
                                IsExactInput = false
                            }.Json,
                            Creator = user,
                            CorrectnessProbability = _random.Next(30, 91),
                            Visibility = QuestionVisibility.Public,
                            Pages = new List<Page> { page }
                        };

                        questionWritingRepository.Create(question);
                        _questions.Add(question);
                        _questionsPerUser[user.Id].Add(question);
                    }
                }
            }
        }

        await Task.CompletedTask;
    }

    private List<QuestionContentDefinition> GetQuestionsForPage(Page page)
    {
        // Find the wiki that contains this page
        foreach (var (wikiPageId, wiki) in _wikiContent)
        {
            // Check if page is the main page
            if (wiki.MainPage.Title == page.Name)
            {
                return wiki.Questions.Where(q => q.PageTitle == page.Name).ToList();
            }

            // Check if page is a subpage
            if (wiki.Subpages.Any(sp => sp.Title == page.Name))
            {
                return wiki.Questions.Where(q => q.PageTitle == page.Name).ToList();
            }
        }

        return new List<QuestionContentDefinition>();
    }

    private async Task GenerateLearningHistoryAsync(int userId)
    {
        if (_questions.Count == 0)
        {
            _performanceLogger.Log("Skipping learning history - no questions available");
            return;
        }

        var maxQuestions = Math.Min(20, _questions.Count);
        var shuffledQuestions = _questions.OrderBy(_ => _random.Next()).Take(maxQuestions).ToList();
        var answerRepository = _testHarness.R<AnswerRepo>();

        DateTimeOffset startDate = _configuration.Now.AddDays(-21);
        List<DateTimeOffset> days = new();

        for (int dayOffset = 0; dayOffset < 21; dayOffset++)
        {
            if (_random.NextDouble() < 0.4)
                days.Add(startDate.AddDays(dayOffset));
        }

        // Ensure at least five active days
        while (days.Count < 5)
            days.Add(startDate.AddDays(_random.Next(0, 21)));

        days = days.OrderBy(date => date).ToList();

        foreach (var day in days)
        {
            int questionCount = _random.Next(3, Math.Min(8, shuffledQuestions.Count + 1));
            var dailyQuestions = shuffledQuestions
                .OrderBy(_ => _random.Next())
                .Take(questionCount)
                .ToList();

            foreach (var question in dailyQuestions)
            {
                DateTimeOffset answerTime = day
                    .AddHours(_random.Next(8, 23))
                    .AddMinutes(_random.Next(0, 60))
                    .AddSeconds(_random.Next(0, 60));

                var answer = new Answer
                {
                    Question = question,
                    UserId = userId,
                    QuestionViewGuid = Guid.NewGuid(),
                    InteractionNumber = 1,
                    MillisecondsSinceQuestionView = _random.Next(1_000, 15_001),
                    AnswerText = _random.NextDouble() < 0.8 ? question.Solution : "Incorrect answer",
                    AnswerredCorrectly = _random.NextDouble() < 0.8 ? AnswerCorrectness.True : AnswerCorrectness.False,
                    DateCreated = answerTime.DateTime
                };
                answerRepository.Create(answer);
            }
        }

        await Task.CompletedTask;
    }
}
