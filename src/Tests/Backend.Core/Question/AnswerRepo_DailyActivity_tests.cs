internal class AnswerRepo_DailyActivity_tests : BaseTestHarness
{
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
