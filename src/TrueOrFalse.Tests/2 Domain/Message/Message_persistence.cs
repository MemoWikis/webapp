using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence;

[Category(TestCategories.Programmer)]
public class Message_persistence : BaseTest
{
    [Test]
    public void Should_persist_message()
    {
        var context = ContextUser.New().Add("Jule").Persist();
        var userId = context.All.First().Id;

        CustomMsg.Send(userId, "subject", "body");
    }
}