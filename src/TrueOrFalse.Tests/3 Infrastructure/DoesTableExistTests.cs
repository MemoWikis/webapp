using NUnit.Framework;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Tests;

public class DoesTableExistTests : BaseTest
{
    [Test]
    public void Should_perform_table_exists()
    {
        Assert.That(Resolve<DoesTableExist>().Run("Some_unknown_table"), Is.False);
        Assert.That(Resolve<DoesTableExist>().Run("Category"), Is.True);
    }
}