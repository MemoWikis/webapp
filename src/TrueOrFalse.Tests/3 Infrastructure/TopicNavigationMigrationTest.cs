using System;
using System.Collections;
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
    public List<Category> dummyList => new List<Category>
    {
        new Category() { Id = 1, Name = "Test1"},
        new Category() { Id = 2, Name = "Test2"},
        new Category() { Id = 3, Name = "Test3"}
    };
    [Test]
    public void Should_remove_TopicNavigation()
    {
        var testString = "[{\"TemplateName\":\"TopicNavigation\"}]";
        var testString2 = "[{\"TemplateName\":\"TopicNavigation\",\"Load\":\"All\"}]";
        var expectedResult = "";

        var newContent = TopicNavigationMigration.RemoveTopicNavigation(testString, dummyList);
        var newContent2 = TopicNavigationMigration.RemoveTopicNavigation(testString2, dummyList);
        Assert.That(newContent, Is.EqualTo(expectedResult));
        Assert.That(newContent2, Is.EqualTo(expectedResult));
    }

    [Test]
    public void Should_Keep_InlineText()
    {
        var testString =
            "[{\"TemplateName\":\"InlineText\",\"Content\":\"<p>Allgemeinwissen bezeichnet umgangssprachlich einen Wissenskanon, der allgemein vorausgesetzt werden kann oder zumindest angestrebt ist unabhängig von der eigenen Fachrichtung. Was genau dazu gehört, ist jedoch je nach Kontext, Interesse und anderen Faktoren unterschiedlich und immer umstritten. Allgemeinwissen gilt als Gegenstück zum Spezialwissen bzw. Spezialisierungswissen.</p>\\n<h3 id=\\\"jahre-reformation\\\"><a href=\\\"https://memucho.de/Kategorien/Reformation/830\\\">500 Jahre Reformation</a></h3>\\n\"}]";
        var testString2 =
            "[{\"TemplateName\":\"TopicNavigation\"},{\"TemplateName\":\"InlineText\",\"Content\":\"<p>Allgemeinwissen bezeichnet umgangssprachlich einen Wissenskanon, der allgemein vorausgesetzt werden kann oder zumindest angestrebt ist unabhängig von der eigenen Fachrichtung. Was genau dazu gehört, ist jedoch je nach Kontext, Interesse und anderen Faktoren unterschiedlich und immer umstritten. Allgemeinwissen gilt als Gegenstück zum Spezialwissen bzw. Spezialisierungswissen.</p>\\n<h3 id=\\\"jahre-reformation\\\"><a href=\\\"https://memucho.de/Kategorien/Reformation/830\\\">500 Jahre Reformation</a></h3>\\n\"}]";
        var testString3 =
            "[{\"TemplateName\":\"TopicNavigation\"},{\"TemplateName\":\"InlineText\",\"Content\":\"<p>Allgemeinwissen bezeichnet umgangssprachlich einen Wissenskanon, der allgemein vorausgesetzt werden kann oder zumindest angestrebt ist unabhängig von der eigenen Fachrichtung. Was genau dazu gehört, ist jedoch je nach Kontext, Interesse und anderen Faktoren unterschiedlich und immer umstritten. Allgemeinwissen gilt als Gegenstück zum Spezialwissen bzw. Spezialisierungswissen.</p>\\n<h3 id=\\\"jahre-reformation\\\"><a href=\\\"https://memucho.de/Kategorien/Reformation/830\\\">500 Jahre Reformation</a></h3>\\n\"},{\"TemplateName\":\"TopicNavigation\",\"Load\":\"All\"}]";
        var expectedResult = "<p>Allgemeinwissen bezeichnet umgangssprachlich einen Wissenskanon, der allgemein vorausgesetzt werden kann oder zumindest angestrebt ist unabhängig von der eigenen Fachrichtung. Was genau dazu gehört, ist jedoch je nach Kontext, Interesse und anderen Faktoren unterschiedlich und immer umstritten. Allgemeinwissen gilt als Gegenstück zum Spezialwissen bzw. Spezialisierungswissen.</p>\n<h3 id=\"jahre-reformation\"><a href=\"https://memucho.de/Kategorien/Reformation/830\">500 Jahre Reformation</a></h3>\n";
        
        var newContent = TopicNavigationMigration.RemoveTopicNavigation(testString, dummyList) ; 
        var newContent2 = TopicNavigationMigration.RemoveTopicNavigation(testString2, dummyList); 
        var newContent3 = TopicNavigationMigration.RemoveTopicNavigation(testString2, dummyList); 
        Assert.That(newContent, Is.EqualTo(expectedResult));
        Assert.That(newContent2, Is.EqualTo(expectedResult));
        Assert.That(newContent3, Is.EqualTo(expectedResult));
    }

    [Test]
    public void Should_get_Urls()
    {
        var testString = "[{\"TemplateName\":\"TopicNavigation\",\"Load\":\"1,2\"}]";
        var newContent = TopicNavigationMigration.RemoveTopicNavigation(testString, dummyList);
        var expectedResult =
            "<p><ul>\n<li><a href=\"/Test1/1\">Test1</a></li>\n<li><a href=\"/Test2/2\">Test2</a></li></p></ul>";
        Assert.That(newContent, Is.EqualTo(expectedResult));
    }
}

