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
        var newContent2 = TopicNavigationMigration.RemoveTopicNavigation(testString2);
        Assert.That(newContent, Is.EqualTo(expectedResult));
        Assert.That(newContent2, Is.EqualTo(expectedResult));
    }

    [Test]
    public void RemoveTopicNavigationEndingWithInlineText()
    {
        var testString =
            "[{\"TemplateName\":\"InlineText\",\"Content\":\"<p>Allgemeinwissen bezeichnet umgangssprachlich einen Wissenskanon, der allgemein vorausgesetzt werden kann oder zumindest angestrebt ist unabhängig von der eigenen Fachrichtung. Was genau dazu gehört, ist jedoch je nach Kontext, Interesse und anderen Faktoren unterschiedlich und immer umstritten. Allgemeinwissen gilt als Gegenstück zum Spezialwissen bzw. Spezialisierungswissen.</p>\\n<h3 id=\\\"jahre-reformation\\\"><a href=\\\"https://memucho.de/Kategorien/Reformation/830\\\">500 Jahre Reformation</a></h3>\\n\"}]";
        var expectedResult = "<p>Allgemeinwissen bezeichnet umgangssprachlich einen Wissenskanon, der allgemein vorausgesetzt werden kann oder zumindest angestrebt ist unabhängig von der eigenen Fachrichtung. Was genau dazu gehört, ist jedoch je nach Kontext, Interesse und anderen Faktoren unterschiedlich und immer umstritten. Allgemeinwissen gilt als Gegenstück zum Spezialwissen bzw. Spezialisierungswissen.</p>\n<h3 id=\"jahre-reformation\"><a href=\"https://memucho.de/Kategorien/Reformation/830\">500 Jahre Reformation</a></h3>\n";
        
        var newContent = TopicNavigationMigration.RemoveTopicNavigation(testString); 
        Assert.That(newContent, Is.EqualTo(expectedResult));

    }
}

