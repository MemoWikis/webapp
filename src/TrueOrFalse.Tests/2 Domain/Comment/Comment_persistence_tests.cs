using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence;

public class Comment_persistence_tests : BaseTest
{
    [Test]
    public void Comments_should_be_persisted()
    {
        var userWritingRepo = R<UserWritingRepo>(); 
        var context =  new ContextComment(R<CommentRepository>(), userWritingRepo );
        context.Add("A").Add("B").Persist();
        context.Add("C", commentTo:context.All[0]).Persist();

        var allComments = Resolve<CommentRepository>().GetAll();
        Assert.That(allComments.Count, Is.EqualTo(3));

        RecycleContainer();

        var question =  ContextQuestion.New(R<QuestionWritingRepo>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            userWritingRepo,
            R<CategoryRepository>())
            .AddQuestion(questionText: "text", solutionText: "solution").Persist().All[0];

        var comments = Resolve<CommentRepository>().GetForDisplay(question.Id);
        Assert.That(comments.First(c => c.Text == "A").Answers[0].Text, Is.EqualTo("C"));
    }
}