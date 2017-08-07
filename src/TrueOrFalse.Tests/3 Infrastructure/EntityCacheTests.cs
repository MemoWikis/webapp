using System;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class EntityCacheTests
{
    [Test]
    public void Test()
    {
        var x = EntityCache.CategoryQuestionsList;
        var y = EntityCache.CategoryQuestionInSetList;
    }
}