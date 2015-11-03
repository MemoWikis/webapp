using NUnit.Framework;

public class AnswerTest_AlgoTester : BaseTest
{
	[Test]
	public void Should_perform_algo_test()
	{
        var ctx = ContextHistory.New();
        ctx.WriteHistory();
        ctx.WriteHistory();
        ctx.WriteHistory();

        AlgoTester.Run();

        RecycleContainer();

        Assert.That(
            Sl.R<AnswerTestRepo>().GetAll().Count, 
            Is.EqualTo(3 * AlgoInfoRepo.GetAll().Count));

        AlgoTester.Run();
	}
}