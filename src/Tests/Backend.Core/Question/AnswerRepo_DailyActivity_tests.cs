internal class AnswerRepo_DailyActivity_tests : BaseTestHarness
{
    [Test]
    public void GetDailyActivityForUser_returns_grouped_counts()
    {
        // Arrange
        var context = NewQuestionContext(persistImmediately: true);
        context.AddQuestion(persistImmediately: true);
        context.AddAnswer("correct answer");
        context.AddAnswer("another answer");

        var answerRepo = R<AnswerRepo>();
        var startDate = DateTime.Today.AddDays(-1);

        // Act
        var result = answerRepo.GetDailyActivityForUser(context.Learner.Id, startDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));

        var todayEntry = result.FirstOrDefault(r => r.Day.Date == DateTime.Today);
        Assert.That(todayEntry, Is.Not.Null);
        Assert.That(todayEntry!.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetDailyActivityForUser_excludes_solution_views()
    {
        // Arrange
        var context = NewQuestionContext(persistImmediately: true);
        context.AddQuestion(persistImmediately: true);
        context.AddAnswer("correct answer");

        // Manually create an IsView answer
        var answerRepo = R<AnswerRepo>();
        var viewAnswer = new Answer
        {
            UserId = context.Learner.Id,
            Question = context.All.Last(),
            AnswerredCorrectly = AnswerCorrectness.IsView,
            MillisecondsSinceQuestionView = 0,
            InteractionNumber = 1,
        };
        answerRepo.Create(viewAnswer);
        answerRepo.Flush();

        var startDate = DateTime.Today.AddDays(-1);

        // Act
        var result = answerRepo.GetDailyActivityForUser(context.Learner.Id, startDate);

        // Assert
        var todayEntry = result.FirstOrDefault(r => r.Day.Date == DateTime.Today);
        Assert.That(todayEntry, Is.Not.Null);
        Assert.That(todayEntry!.Count, Is.EqualTo(1));
    }

    [Test]
    public void GetDailyActivityForUser_returns_empty_for_no_activity()
    {
        // Arrange
        var context = NewQuestionContext(persistImmediately: true);
        var answerRepo = R<AnswerRepo>();
        var startDate = DateTime.Today.AddDays(-1);

        // Act
        var result = answerRepo.GetDailyActivityForUser(context.Learner.Id, startDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetDailyActivityForUserOnPage_returns_page_specific_counts()
    {
        // Arrange
        var pageRepository = R<PageRepository>();
        var page = new Page { Name = "TestPage", Creator = null! };
        pageRepository.Create(page);
        pageRepository.Flush();

        var context = NewQuestionContext(persistImmediately: true);
        context.AddQuestion(pages: new List<Page> { page }, persistImmediately: true);
        context.AddAnswer("answer on page");

        // Create another question NOT on the page
        context.AddQuestion(persistImmediately: true);
        context.AddAnswer("answer not on page");

        var answerRepo = R<AnswerRepo>();
        var startDate = DateTime.Today.AddDays(-1);

        // Act
        var result = answerRepo.GetDailyActivityForUserOnPage(context.Learner.Id, page.Id, startDate);

        // Assert
        var todayEntry = result.FirstOrDefault(r => r.Day.Date == DateTime.Today);
        Assert.That(todayEntry, Is.Not.Null);
        Assert.That(todayEntry!.Count, Is.EqualTo(1));
    }
}
