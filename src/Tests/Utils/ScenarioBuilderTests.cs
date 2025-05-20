using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    /// <summary>
    /// Example test class showing how to use the ScenarioBuilder
    /// </summary>
    [TestFixture]
    [Category("ScenarioTests")]
    internal class ScenarioBuilderTests : BaseTestHarness
    {
        [Test]
        [Ignore("for local development")]
        [Explicit("This test creates a full scenario and persists it to a Docker image. Run manually when needed.")]
        public async Task Should_Create_Full_Scenario_And_Persist_To_Docker_Image()
        {
            // Arrange
            var scenarioBuilder = new ScenarioBuilder(_testHarness);

            // Act
            await scenarioBuilder.BuildScenarioAsync();
            string imageName = await scenarioBuilder.PersistToDockerImageAsync("memowikis-test-scenario");

            // Assert
            var users = await _testHarness.DbData.AllUsersAsync();
            var pages = await _testHarness.DbData.AllPagesAsync();
            var questions = await _testHarness.DbData.AllQuestionsAsync();

            Assert.That(users.Count, Is.GreaterThanOrEqualTo(3), "Should have at least 3 users");
            Assert.That(pages.Count, Is.GreaterThan(5), "Should have multiple pages");
            Assert.That(questions.Count, Is.GreaterThan(10), "Should have multiple questions");

            Console.WriteLine($"Created Docker image: {imageName}");
            Console.WriteLine($"Users created: {users.Count}");
            Console.WriteLine($"Pages created: {pages.Count}");
            Console.WriteLine($"Questions created: {questions.Count}");
        }

        [Test]
        [Ignore("for local development")]
        [Explicit("This test demonstrates loading a pre-created scenario Docker image")]
        public async Task Should_Load_Scenario_From_Docker_Image()
        {
            // For demonstration only - would need existing image
            string imageName = "memowikis-test-scenario:20250520123456"; // Use a real image name here

            // Act
            bool success = await ScenarioBuilder.LoadDockerImageAsync(imageName);

            // Assert
            Assert.That(success, Is.True, "Should successfully load the Docker image");
        }

        [Test]
        public async Task Should_Create_Users_With_Pages_And_Questions()
        {
            // Arrange
            var scenarioBuilder = new ScenarioBuilder(_testHarness);

            // Act
            await scenarioBuilder.BuildScenarioAsync();
            var users = await _testHarness.DbData.AllUsersAsync();
            var pages = await _testHarness.DbData.AllPagesAsync();
            var questions = await _testHarness.DbData.AllQuestionsAsync();

            // Verify users
            Assert.That(users.Count, Is.GreaterThanOrEqualTo(3), "Should have at least 3 users");
            Assert.That(users.Any(u => u["Name"]?.ToString() == "LearningUser"), "Should have a user named LearningUser");
            Assert.That(users.Any(u => u["Name"]?.ToString() == "ContentCreator"), "Should have a user named ContentCreator");
            Assert.That(users.Any(u => u["Name"]?.ToString() == "QuestionContributor"), "Should have a user named QuestionContributor");

            // Verify pages
            Assert.That(pages.Count, Is.GreaterThan(5), "Should have multiple pages");

            // Verify questions
            Assert.That(questions.Count, Is.GreaterThan(10), "Should have multiple questions");
        }

        [Test]
        [Ignore("for local development")]
        public async Task Should_Create_Learning_History_For_First_User()
        {
            // Arrange
            var scenarioBuilder = new ScenarioBuilder(_testHarness);

            // Act
            await scenarioBuilder.BuildScenarioAsync();            // Assert
            var users = await _testHarness.DbData.AllUsersAsync();
            var user = users.First(u => u["Name"]?.ToString() == "LearningUser");
            var userId = Convert.ToInt32(user["Id"]);

            // Execute custom SQL to get answers data
            // Create a repository for Answer instead
            var answerRepo = R<AnswerRepo>();
            var answers = answerRepo.GetAll();

            Assert.That(answers.Count, Is.GreaterThan(10), "Should have learning history entries for the first user");
            var dates = answers.Select(a => a.DateCreated.Date).Distinct().OrderBy(d => d).ToList();

            Assert.That(dates.Count, Is.GreaterThanOrEqualTo(5), "Should have answers on at least 5 different days");

            // Check for at least one gap in the learning history (non-consecutive days)
            bool hasGap = false;
            for (int i = 1; i < dates.Count; i++)
            {
                if ((dates[i] - dates[i - 1]).TotalDays > 1)
                {
                    hasGap = true;
                    break;
                }
            }

            Assert.That(hasGap, Is.True, "Should have gaps in learning history (sporadic learning pattern)");
        }
    }
}
