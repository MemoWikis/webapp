

class Relation_tests : BaseTest
{
    [Test]
    public void Should_Add_Creation_ToDb_and_EntityCache()
    {
        var context = ContextCategory.New();

        context.Add("RootElement").Persist();

        context
            .Add("Sub1")
            .Persist();




    }

}
