namespace TrueOrFalse.Tests8._0
{
    public class Tests : BaseTest
    {
        public Tests()
        {
        }

        [Test]
        public void Test1()
        {
            ContextCategory.New().Add("A").Persist();
        }
    }
}