using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using TemplateMigration;


[TestFixture]
class TopicNavigationMigrationTest
{
    [Test]
    public void Should_remove_TopicNavigation()
    {
        var testString = "[{\"TemplateName\":\"TopicNavigation\"}]";
        var testString2 = "[{\"TemplateName\":\"TopicNavigation\",\"Load\":\"All\"}]";
        var expectedResult = "";

        var newContent = TopicNavigationMigration.RemoveTopicNavigation(testString);
        Assert.That(newContent, Is.EqualTo(expectedResult));
    }

    //[Test]
    //public void RemoveTopicNavigationEndingWithInlineText()
    //{
    //    var testString = "[{\"TemplateName\":\"TopicNavigation\"},{ \"TemplateName\":\"InlineText\",\"Content\":\"<h2 id=\"weiterfuhrende-links\">Weiterführende Links</h2>\n<p><a href=\"https://de.wikipedia.org/wiki/Containervirtualisierung\">https://de.wikipedia.org/wiki/Containervirtualisierung</a></p>\n\"}]";
    //    var expectedResult = "[{ \"TemplateName\":\"InlineText\",\"Content\":\"<h2 id=\"weiterfuhrende-links\">Weiterführende Links</h2>\n<p><a href=\"https://de.wikipedia.org/wiki/Containervirtualisierung\">https://de.wikipedia.org/wiki/Containervirtualisierung</a></p>\n\"}]";

    //}
}

