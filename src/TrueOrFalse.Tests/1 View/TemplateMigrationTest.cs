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

    [Test]
    public void Should_do_nothing_if_no_DivStart()
    {
        var input =
            "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]\r\n" +
            "[[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]\r\n" +
            "[[{\"TemplateName\":\"Test\"}]]";

        var result = "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]\r\n" +
                "[[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]\r\n" +
                "[[{\"TemplateName\":\"Test\"}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_add_NewLine_between_Templates()
    {
        var input =
            "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]][[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]";

        var result = "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]\r\n" +
                     "[[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_reduce_NewLine_between_Templates()
    {
        var input =
            "\r\n[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]\r\n\r\n\r\n[[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]";

        var result = "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]\r\n" +
                     "[[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_trim_whitespaces_between_Templates()
    {
        var input =
            "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]       [[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]";

        var result = "[[{\"TemplateName\":\"Test\", \"CssClasses\":\"CardsLandscape\"}]]\r\n" +
                     "[[{\"TemplateName\": \"Foo\",\"SetId\": 20}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_remove_invalid_data_in_cards_template()
    {
        var input =
            "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"row CardsLandscape\"}]]" +
            "\r\n[[{\"TemplateName\": \"SingleSet\",\"SetId\": 20}]]" +
            "\r\n\r\n asd[[{\"TemplateName\": \"SingleSet\",\"SetId\": 18}]]" +
            "[[{\"TemplateName\":\"DivEnd\"}]]";

        var result = "[[{\"TemplateName\":\"Cards\",\"CardOrientation\":\"Landscape\",\"SetListIds\":\"20,18\"}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_remove_invalid_data_in_cards_template_but_keep_structure_outside_template()
    {
        var input =
            "[[{\"TemplateName\":\"DivStart\", \"CssClasses\":\"row CardsLandscape\"}]]" +
            "\r\n[[{\"TemplateName\": \"SingleSet\",\"SetId\": 20}]]" +
            "\r\n\r\n asd[[{\"TemplateName\": \"SingleSet\",\"SetId\": 18}]]" +
            "[[{\"TemplateName\":\"DivEnd\"}]]" +
            "\r\n Lorem Ipsum []";

        var result = "[[{\"TemplateName\":\"Cards\",\"CardOrientation\":\"Landscape\",\"SetListIds\":\"20,18\"}]]\r\n Lorem Ipsum []";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }

    [Test]
    public void Should_not_convert_singlesetfullwidth()
    {
        var input = "[[{\"TemplateName\":\"SingleSetFullWidth\", \"SetId\":\"12\"}]]";

        var result = "[[{\"TemplateName\":\"SingleSetFullWidth\", \"SetId\":\"12\"}]]";
        Assert.That(MarkdownConverter.ConvertMarkdown(input), Is.EqualTo(result));
    }
}