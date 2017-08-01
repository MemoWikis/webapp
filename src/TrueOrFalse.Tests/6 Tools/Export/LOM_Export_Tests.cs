using System;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class LOM_Export_Tests : BaseTest
{
    [Test]
    public void Should_export_LOM()
    {
        var category = ContextCategory.New().Add("Hello LOM").Persist().All.First();
        var lomXml = category.ToLomXml();

        Console.Write(lomXml);
    }
}

