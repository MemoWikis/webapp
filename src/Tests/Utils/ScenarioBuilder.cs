/// <summary>
/// ScenarioBuilder creates a comprehensive test scenario with users, pages, and questions.
/// It can be optionally called to set up test data and persist it as a Docker image.
/// </summary>
public class ScenarioBuilder(TestHarness _testHarness)
{
    private readonly Random _random = new();
    private readonly List<User> _users = new();
    private readonly List<Page> _pages = new();
    private readonly List<Question> _questions = new();
    private readonly Dictionary<int, List<Page>> _userPages = new();
    private readonly Dictionary<int, List<Question>> _userQuestions = new();

    // Dictionary to keep track of page hierarchy (parent -> list of children)
    private readonly Dictionary<int, List<int>> _pageHierarchy = new();

    /// <summary>
    /// Sets up the scenario with 3 users, each with pages and questions.
    /// One user will have learning history with sporadic activity.
    /// </summary>
    public async Task BuildScenarioAsync()
    {
        await CreateUsersAsync();
        await CreatePagesAsync();
        await CreateQuestionsAsync();


        await GenerateLearningHistoryAsync(_users[0].Id);
    }

    /// <summary>
    /// Creates three users with different characteristics
    /// </summary>
    private async Task CreateUsersAsync()
    {
        var userWritingRepo = _testHarness.R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        // User 1: Regular user with learning history
        var user1 = new User
        {
            Name = "LearningUser",
            EmailAddress = "learning.user@example.com",
            IsEmailConfirmed = true,
            DateCreated = DateTime.Now.AddMonths(-1),
        };
        contextUser.Add(user1);

        // User 2: Content creator with many pages
        var user2 = new User
        {
            Name = "ContentCreator",
            EmailAddress = "content.creator@example.com",
            IsEmailConfirmed = true,
            DateCreated = DateTime.Now.AddMonths(-2),
        };
        contextUser.Add(user2);

        // User 3: Question contributor 
        var user3 = new User
        {
            Name = "QuestionContributor",
            EmailAddress = "question.contributor@example.com",
            IsEmailConfirmed = true,
            DateCreated = DateTime.Now.AddMonths(-3),
        };
        contextUser.Add(user3);

        contextUser.Persist();
        _users.AddRange(contextUser.All);

        // Initialize user dictionaries
        foreach (var user in _users)
        {
            _userPages[user.Id] = new List<Page>();
            _userQuestions[user.Id] = new List<Question>();
        }
    }

    /// <summary>
    /// Creates pages with nested structure for each user
    /// </summary>
    private async Task CreatePagesAsync()
    {
        var pageRepository = _testHarness.R<PageRepository>();
        var pageRelationRepo = _testHarness.R<PageRelationRepo>();
        var contextPage = new ContextPage(_testHarness, false);

        // User 1: Create basic wiki structure with some nesting
        await CreateUserPagesAsync(_users[0], contextPage, 3, 2, "Learning");

        // User 2: Create extensive wiki structure with deep nesting
        await CreateUserPagesAsync(_users[1], contextPage, 5, 3, "Knowledge");

        // User 3: Create minimal wiki structure
        await CreateUserPagesAsync(_users[2], contextPage, 2, 1, "Questions");

        _pages.AddRange(contextPage.All);
    }

    /// <summary>
    /// Create pages for a specific user with a hierarchical structure
    /// </summary>
    /// <param name="user">The user who creates the pages</param>
    /// <param name="contextPage">The context page helper</param>
    /// <param name="topLevelPages">Number of top-level pages to create</param>
    /// <param name="nestingDepth">Max depth of page nesting</param>
    /// <param name="topicPrefix">Prefix for page names</param>
    private async Task CreateUserPagesAsync(User user, ContextPage contextPage, int topLevelPages, int nestingDepth, string topicPrefix)
    {
        // Create top-level pages
        List<Page> rootPages = new List<Page>();
        for (int i = 1; i <= topLevelPages; i++)
        {
            string pageName = $"{topicPrefix} Topic {i}";
            contextPage
                .Add(pageName, user, PageVisibility.Public, isWiki: true)
                .Persist();
            
            var page = contextPage.All.Last();
            rootPages.Add(page);
            _userPages[user.Id].Add(page);
            _pageHierarchy[page.Id] = new List<int>();
        }

        // Create nested pages
        foreach (var rootPage in rootPages)
        {
            await CreateNestedPagesAsync(user, contextPage, rootPage, 1, nestingDepth, topicPrefix);
        }

        contextPage.Persist();
    }

    /// <summary>
    /// Recursively create nested pages under a parent page
    /// </summary>
    private async Task CreateNestedPagesAsync(User user, ContextPage contextPage, Page parentPage, int currentDepth, int maxDepth, string topicPrefix)
    {
        if (currentDepth > maxDepth)
            return;

        // Create 1-3 child pages at each level
        int childCount = _random.Next(1, 4);
        for (int i = 1; i <= childCount; i++)
        {
            string pageName = $"{topicPrefix} Subtopic {parentPage.Id}-{i}";
            contextPage
                .Add(pageName, user, PageVisibility.Public, isWiki: true)
                .Persist();
            
            var childPage = contextPage.All.Last();

            // Add the page relation
            contextPage.AddChild(parentPage, childPage);

            _userPages[user.Id].Add(childPage);
            _pageHierarchy[parentPage.Id].Add(childPage.Id);
            _pageHierarchy[childPage.Id] = new List<int>();

            // Recursively create more nested pages
            await CreateNestedPagesAsync(user, contextPage, childPage, currentDepth + 1, maxDepth, topicPrefix);
        }
    }

    /// <summary>
    /// Creates questions for the pages
    /// </summary>
    private async Task CreateQuestionsAsync()
    {
        var questionWritingRepo = _testHarness.R<QuestionWritingRepo>();
        var pageRepository = _testHarness.R<PageRepository>();

        foreach (var user in _users)
        {
            int questionsPerPage = user.Id == _users[2].Id ? 5 : 2; // More questions for the question contributor

            foreach (var page in _userPages[user.Id])
            {
                for (int i = 1; i <= questionsPerPage; i++)
                {
                    var question = new Question
                    {
                        Text = $"Question {i} for page {page.Name}?",
                        Solution = $"Answer {i} for page {page.Name}",
                        SolutionType = SolutionType.Text,
                        SolutionMetadataJson = new SolutionMetadataText { IsCaseSensitive = false, IsExactInput = false }.Json,
                        Creator = user,
                        CorrectnessProbability = _random.Next(30, 91), // Random probability between 30-90%
                        Visibility = QuestionVisibility.Public,
                        Pages = new List<Page> { page }
                    };

                    questionWritingRepo.Create(question, pageRepository);
                    _questions.Add(question);
                    _userQuestions[user.Id].Add(question);
                }
            }
        }
    }

    /// <summary>
    /// Generates learning history for a user with sporadic learning activity
    /// </summary>
    private async Task GenerateLearningHistoryAsync(int userId)
    {
        var questions = _questions.OrderBy(_ => _random.Next()).Take(20).ToList();
        var user = _users.First(u => u.Id == userId);
        var answerRepo = _testHarness.R<AnswerRepo>();
        var questionReadingRepo = _testHarness.R<QuestionReadingRepo>();

        // Generate sporadic learning history over the past few weeks (not daily)
        var startDate = DateTime.Now.AddDays(-21);  // 3 weeks ago
        var activeDays = new List<DateTime>();

        // Select random days in the past 3 weeks for activity (sporadic, not daily)
        for (int i = 0; i < 21; i++)
        {
            // About 40% chance of activity on any given day
            if (_random.NextDouble() < 0.4)
            {
                activeDays.Add(startDate.AddDays(i));
            }
        }

        // Ensure we have at least 5 active days and some consecutive days
        if (activeDays.Count < 5)
        {
            for (int i = 0; i < 5; i++)
            {
                var day = startDate.AddDays(_random.Next(0, 21));
                if (!activeDays.Contains(day))
                    activeDays.Add(day);
            }
        }

        // Ensure we have at least one set of consecutive days
        bool hasConsecutiveDays = false;
        for (int i = 1; i < activeDays.Count; i++)
        {
            if ((activeDays[i] - activeDays[i - 1]).TotalDays == 1)
            {
                hasConsecutiveDays = true;
                break;
            }
        }

        if (!hasConsecutiveDays)
        {
            var day = startDate.AddDays(_random.Next(0, 19));
            activeDays.Add(day);
            activeDays.Add(day.AddDays(1));
            activeDays.Add(day.AddDays(2));
        }

        activeDays = activeDays.OrderBy(d => d).ToList();

        // Create answers for each active day
        foreach (var day in activeDays)
        {
            // Generate answers for 3-7 questions each day
            var dailyQuestions = questions.OrderBy(_ => _random.Next()).Take(_random.Next(3, 8)).ToList();

            foreach (var question in dailyQuestions)
            {
                // Time between 8:00 AM and 10:00 PM
                var hour = _random.Next(8, 23);
                var minute = _random.Next(0, 60);
                var second = _random.Next(0, 60);
                var answerTime = day.Date.AddHours(hour).AddMinutes(minute).AddSeconds(second);

                var answer = new Answer
                {
                    Question = question,
                    UserId = userId,
                    QuestionViewGuid = Guid.NewGuid(),
                    InteractionNumber = 1,
                    MillisecondsSinceQuestionView = _random.Next(1000, 15000), // 1-15 seconds
                    AnswerText = _random.NextDouble() < 0.8 ? question.Solution : "Incorrect answer",
                    AnswerredCorrectly = _random.NextDouble() < 0.8 ? AnswerCorrectness.True : AnswerCorrectness.False,
                    DateCreated = answerTime
                };

                answerRepo.Create(answer);

                // For some questions, add a view first then an answer
                if (_random.NextDouble() < 0.3)
                {
                    var view = new Answer
                    {
                        Question = question,
                        UserId = userId,
                        QuestionViewGuid = answer.QuestionViewGuid,
                        InteractionNumber = 0,
                        MillisecondsSinceQuestionView = 0,
                        AnswerText = "",
                        AnswerredCorrectly = AnswerCorrectness.IsView,
                        DateCreated = answerTime.AddSeconds(-30)
                    };

                    answerRepo.Create(view);
                }
            }
        }
    }

    /// <summary>
    ///     Creates a Docker image of the current database state that can be saved to a central repository
    /// </summary>
    /// <param name="imageName">Name to give the Docker image</param>
    /// <param name="saveToFile">Whether to save the image to a local file</param>
    /// <returns>Path to the saved Docker image</returns>
    public async Task<string> PersistToDockerImageAsync(string imageName = "memowikis-test-scenario", bool saveToFile = true)
    {
        // Get the Docker container ID for the MySQL container
        string containerId = await GetMySqlContainerIdAsync();
        if (string.IsNullOrEmpty(containerId))
        {
            throw new InvalidOperationException("MySQL Docker container not found.");
        }

        // Create a Docker image from the running container
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string fullImageName = $"{imageName}:{timestamp}";
        await CommitContainerToImageAsync(containerId, fullImageName);

        // Optionally save image to file for later use
        string outputFile = string.Empty;
        if (saveToFile)
        {
            outputFile = $"{imageName}-{timestamp}.tar";
            await SaveDockerImageToFileAsync(fullImageName, outputFile);
        }

        return fullImageName;
    }

    /// <summary>
    ///     Pushes a Docker image to a central repository
    /// </summary>
    /// <param name="imageName">Name of the Docker image to push</param>
    /// <param name="repository">Repository URL (default: Docker Hub)</param>
    /// <returns>True if the push was successful</returns>
    public async Task<bool> PushDockerImageToRepositoryAsync(string imageName, string repository = "")
    {
        // Tag the image with the repository if provided
        if (!string.IsNullOrEmpty(repository))
        {
            var tagStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"tag {imageName} {repository}/{imageName}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var tagProcess = System.Diagnostics.Process.Start(tagStartInfo);
            if (tagProcess == null)
                return false;

            await tagProcess.WaitForExitAsync();
            if (tagProcess.ExitCode != 0)
                return false;

            imageName = $"{repository}/{imageName}";
        }

        // Push the image to the repository
        var pushStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"push {imageName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var pushProcess = System.Diagnostics.Process.Start(pushStartInfo);
        if (pushProcess == null)
            return false;

        await pushProcess.WaitForExitAsync();
        return pushProcess.ExitCode == 0;
    }/// <summary>
     /// Loads a Docker image from a repository or local file
     /// </summary>
     /// <param name="imageNameOrPath">Name of the Docker image or path to the saved image file</param>
     /// <returns>True if the image was successfully loaded</returns>
    public static async Task<bool> LoadDockerImageAsync(string imageNameOrPath)
    {
        string arguments;

        if (imageNameOrPath.EndsWith(".tar"))
        {
            // Load image from file
            arguments = $"load -i {imageNameOrPath}";
        }
        else
        {
            // Pull image from repository
            arguments = $"pull {imageNameOrPath}";
        }

        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            return false;

        await process.WaitForExitAsync();
        return process.ExitCode == 0;
    }

    private async Task<string> GetMySqlContainerIdAsync()
    {
        // Use Docker CLI to find the MySQL container for our tests
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --filter \"name=mysql\" --format \"{{.ID}}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null) return string.Empty;

        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return output.Trim();
    }

    private async Task CommitContainerToImageAsync(string containerId, string imageName)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"commit {containerId} {imageName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start Docker commit process.");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Docker commit failed with exit code {process.ExitCode}.");
    }

    private async Task SaveDockerImageToFileAsync(string imageName, string outputFile)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"save -o {outputFile} {imageName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start Docker save process.");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Docker save failed with exit code {process.ExitCode}.");
    }
}
