using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TemplateMigration;

[TestFixture]
class TemplateMigrationTest
{
    [Test]
    public void Should_convert_template_to_text()
    {
        var inputCenterElement = "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"centerElements\"}]]![](https://www.youtube.com/watch?v=7v8Qknd6J5g)[[{ \"TemplateName\":\"DivEnd\"}]]";
        var inputBox = "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"Box\"}]]![](https://www.youtube.com/watch?v=7v8Qknd6J5g)[[{ \"TemplateName\":\"DivEnd\"}]]";
        var result = "![](https://www.youtube.com/watch?v=7v8Qknd6J5g)";
        Assert.That(MarkdownConverter.ConvertMarkdown(inputCenterElement), Is.EqualTo(result));
        Assert.That(MarkdownConverter.ConvertMarkdown(inputBox), Is.EqualTo(result));
    }

    [Test]
    public void Should_convert_template_with_cardsPortrait_class_to_card_template()
    {
        var input =
            "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"row CardsPortrait\"}]]" +
            "[[{\"TemplateName\": \"SingleSet\",\"SetId\": 20}]]" +
            "[[{  \"TemplateName\": \"SingleSet\"," +
            "\"SetId\": 25}]]" +
            "[[{\"TemplateName\": \"SingleSet\",\"SetId\": 93}]]" +
            "[[{\"TemplateName\":\"DivEnd\"}]]";

        var result = "[[{\"TemplateName\":\"Cards\",\"CardOrientation\":\"Portrait\",\"SetListIds\":\"20,25,93\"}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_convert_template_with_cardsLandscape_class_to_card_template()
    {
        var input =
            "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"row CardsLandscape\"}]]" +
            "[[{\"TemplateName\": \"SingleSet\",\"SetId\": 20}]]" +
            "[[{\"TemplateName\":   \"SingleSet\",\"SetId\": 12}]]" +
            "[[{\"TemplateName\":\"DivEnd\"}]]";

        var result = "[[{\"TemplateName\":\"Cards\",\"CardOrientation\":\"Landscape\",\"SetListIds\":\"20,12\"}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_convert_template_with_cardsLandscape_class_to_card_template_with_One_SingleSet()
    {
        var input =
            "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"row CardsLandscape\"}]]" +
            "[[{\"TemplateName\": \"SingleSet\",\"SetId\": 20}]]" +
            "[[{\"TemplateName\":\"DivEnd\"}]]";

        var result = "[[{\"TemplateName\":\"Cards\",\"CardOrientation\":\"Landscape\",\"SetListIds\":\"20\"}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }
}