using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence;

public class Comment_persistence : BaseTest
{
    [Test]
    public void Comments_should_be_persisted()
    {
        var context = ContextComment.New();
        context.Add("A").Add("B").Persist();
        context.Add("C", commentTo:context.All[0]).Persist();

        var allComments = Resolve<CommentRepository>().GetAll();
        Assert.That(allComments.Count, Is.EqualTo(3));

        RecycleContainer();

        var comments = Resolve<CommentRepository>().GetForDisplay(context.Question.Id);
        Assert.That(comments.First(c => c.Text == "A").Answers[0].Text, Is.EqualTo("C"));
    }
}