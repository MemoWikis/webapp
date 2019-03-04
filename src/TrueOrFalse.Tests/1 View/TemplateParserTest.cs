using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;

[TestFixture]
class TemplateParserTest
{
    [Test]
    public void Parser_should_parse_correct()
    {
        TemplateParser.Run("Test", new Category("test"), new ControllerContext());
    }

    [Test]
    public void Should_parse_markdown()
    {
        var splitMarkdown = MarkdownToHtml.SplitMarkdown("Test Anfang\r\n\r\nTest 2\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\nTest\r\n\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]");

        Assert.That(splitMarkdown.Count(p => p.IsTemplate), Is.EqualTo(2));
        Assert.That(splitMarkdown.Count(p => p.IsText), Is.EqualTo(2));
        Assert.That(splitMarkdown[0].ToText(), Is.EqualTo("Test Anfang\r\n\r\nTest 2\r\n"));
        Assert.That(splitMarkdown[0].Type, Is.EqualTo(PartType.Text));

        var markdownStartsWithText = MarkdownToHtml.SplitMarkdown("Text[[Template]]");

        Assert.That(markdownStartsWithText.Count(p => p.IsTemplate), Is.EqualTo(1));
        Assert.That(markdownStartsWithText.Count(p => p.IsText), Is.EqualTo(1));
        Assert.That(markdownStartsWithText[0].ToText(), Is.EqualTo("Text"));
        Assert.That(markdownStartsWithText[0].Type, Is.EqualTo(PartType.Text));
    }

    [Test]
    public void Should_parse_markdown_Template_Start()
    {
        var markdownEndsWithText = MarkdownToHtml.SplitMarkdown("[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]] Text");

        Assert.That(markdownEndsWithText.Count(p => p.IsTemplate), Is.EqualTo(1));
        Assert.That(markdownEndsWithText.Count(p => p.IsText), Is.EqualTo(1));
        Assert.That(markdownEndsWithText[0].ToText(), Is.EqualTo("[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]"));
        Assert.That(markdownEndsWithText[1].ToText(), Is.EqualTo(" Text"));
        Assert.That(markdownEndsWithText[0].Type, Is.EqualTo(PartType.Template));
        Assert.That(markdownEndsWithText[1].Type, Is.EqualTo(PartType.Text));
    }

    [Test]
    public void Should_parse_markdown_multiple_Templates()
    {
        var markdownWithMultiTemplates = MarkdownToHtml.SplitMarkdown("Text[[Template]][[Template]]");

        Assert.That(markdownWithMultiTemplates.Count(p => p.IsTemplate), Is.EqualTo(2));
    }

    [Test]
    public void Should_parse_markdown_multiple_Texts()
    {
        var markdownWithMultiTexts = MarkdownToHtml.SplitMarkdown("Text[[Template]]Test");

        Assert.That(markdownWithMultiTexts.Count(p => p.IsText), Is.EqualTo(2));

    }

    [Test]
    public void Should_parse_markdown_with_single_brackets()
    {
        var markdownWithSingleBrackets = MarkdownToHtml.SplitMarkdown("[Text][[Template]]");

        Assert.That(markdownWithSingleBrackets.Count(p => p.IsText), Is.EqualTo(1));
        Assert.That(markdownWithSingleBrackets[0].ToText(), Is.EqualTo("[Text]"));
        Assert.That(markdownWithSingleBrackets[0].Type, Is.EqualTo((PartType.Text)));
    }
}
