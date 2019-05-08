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
        MarkdownSingleTemplateToHtml.Run("Test", new Category("test"), new ControllerContext());
    }

    [Test]
    public void Parser_should_parse_raw_text_to_html()
    {
        var splitMarkdown = MarkdownTokenizer.Run("Test Anfang\r\n\r\nTest 2\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\nTest\r\n\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]");

        var html = MarkdownSingleTemplateToHtml.Run(splitMarkdown.First(), new Category(), new ControllerContext());

        Assert.IsTrue(html.StartsWith("<content-module inline-template"));
    }

    [Test]
    public void Parser_should_parse_text()
    {
//        var html = MarkdownSingleTemplateToHtml.Run("Test", new Category("test"), new ControllerContext());
    }

    [Test]
    public void Should_parse_markdown()
    {
        var splitMarkdown = MarkdownTokenizer.Run("Test Anfang\r\n\r\nTest 2\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\nTest\r\n\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]");

        Assert.That(splitMarkdown.Count(p => p.IsTemplate), Is.EqualTo(2));
        Assert.That(splitMarkdown.Count(p => p.IsText), Is.EqualTo(2));
        Assert.That(splitMarkdown[0].ToText(), Is.EqualTo("Test Anfang\r\n\r\nTest 2\r\n"));
        Assert.That(splitMarkdown[0].Type, Is.EqualTo(TokenType.Text));

        var markdownStartsWithText = MarkdownTokenizer.Run("Text[[Template]]");

        Assert.That(markdownStartsWithText.Count(p => p.IsTemplate), Is.EqualTo(1));
        Assert.That(markdownStartsWithText.Count(p => p.IsText), Is.EqualTo(1));
        Assert.That(markdownStartsWithText[0].ToText(), Is.EqualTo("Text"));
        Assert.That(markdownStartsWithText[0].Type, Is.EqualTo(TokenType.Text));
    }

    [Test]
    public void Should_parse_markdown_Template_Start()
    {
        var markdownEndsWithText = MarkdownTokenizer.Run("[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]] Text");

        Assert.That(markdownEndsWithText.Count(p => p.IsTemplate), Is.EqualTo(1));
        Assert.That(markdownEndsWithText.Count(p => p.IsText), Is.EqualTo(1));
        Assert.That(markdownEndsWithText[0].ToText(), Is.EqualTo("[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]"));
        Assert.That(markdownEndsWithText[1].ToText(), Is.EqualTo(" Text"));
        Assert.That(markdownEndsWithText[0].Type, Is.EqualTo(TokenType.Template));
        Assert.That(markdownEndsWithText[1].Type, Is.EqualTo(TokenType.Text));
    }

    [Test]
    public void Should_parse_markdown_multiple_Templates()
    {
        var markdownWithMultiTemplates = MarkdownTokenizer.Run("Text[[Template]][[Template]]");

        Assert.That(markdownWithMultiTemplates.Count(p => p.IsTemplate), Is.EqualTo(2));
    }

    [Test]
    public void Should_remove_newLines_and_whitespace_between_Templates_if_theres_nothing_else()
    {
        var input = "Text[[Template]]\r\n  [[Template]]";
        var markdownWithMultiTemplates = MarkdownTokenizer.Run(input);

        Assert.That(markdownWithMultiTemplates.Count(p => p.IsTemplate), Is.EqualTo(2));
        Assert.That(markdownWithMultiTemplates.Count(p => p.IsText), Is.EqualTo(2));
    }

    [Test]
    public void Should_parse_markdown_multiple_Templates_and_start_with_Template()
    {
        var markdownWithMultiTemplates = MarkdownTokenizer.Run("[[Template]][[Template]]");

        Assert.That(markdownWithMultiTemplates.Count(p => p.IsTemplate), Is.EqualTo(2));
    }

    [Test]
    public void Should_parse_markdown_multiple_Texts()
    {
        var markdownWithMultiTexts = MarkdownTokenizer.Run("Text[[Template]]Test");

        Assert.That(markdownWithMultiTexts.Count(p => p.IsText), Is.EqualTo(2));

    }

    [Test]
    public void Should_parse_markdown_with_single_brackets()
    {
        var markdownWithSingleBrackets = MarkdownTokenizer.Run("[Text][[Template]]");

        Assert.That(markdownWithSingleBrackets.Count(p => p.IsText), Is.EqualTo(1));
        Assert.That(markdownWithSingleBrackets[0].ToText(), Is.EqualTo("[Text]"));
        Assert.That(markdownWithSingleBrackets[0].Type, Is.EqualTo((TokenType.Text)));
    }

    [Test]
    public void Should_extract_description()
    {
        var document = TemplateParser.Run(
            "Test Anfang\r\n\r\nTest 2\r\n[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\n" +
            "Test\r\n\r\n" +
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]",
            new Category());

        var description = Document.GetDescription(document);

        Assert.That(document.Elements[0].Type, Is.EqualTo("inlinetext"));
        Assert.That(document.Elements.Count(e => e.IsText), Is.EqualTo(2));
        Assert.That(description, Is.EqualTo("Test Anfang\r\n\r\nTest 2\r\nTest"));
    }

    [Test]
    public void Should_extract_description_without_templates()
    {
        var document = TemplateParser.Run(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor",
            new Category());

        var description = Document.GetDescription(document);

        Assert.That(document.Elements[0].Type, Is.EqualTo("inlinetext"));
        Assert.That(document.Elements.Count(e => e.IsText), Is.EqualTo(1));
        Assert.That(description, Is.EqualTo("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor"));
    }

    [Test]
    public void Should_extract_description_start_with_template()
    {
        var document = TemplateParser.Run(
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\n" +
            "Lorem Ipsum test usw und so fooort\r\n\r\n" +
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]",
            new Category());

        var description = Document.GetDescription(document);

        Assert.That(document.Elements[0].Type, Is.EqualTo("cards"));
        Assert.That(document.Elements.Count(e => e.IsText), Is.EqualTo(1));
        Assert.That(description, Is.EqualTo("Lorem Ipsum test usw und so fooort"));
    }

    [Test]
    public void Should_extract_description_with_text_over_150_char()
    {
        var document = TemplateParser.Run(
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\n" +
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." +
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]",
            new Category());

        var description = Document.GetDescription(document);

        Assert.That(document.Elements[0].Type, Is.EqualTo("cards"));
        Assert.That(document.Elements.Count(e => e.IsText), Is.EqualTo(1));
        Assert.That(description, Is.EqualTo("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500..."));
    }

    [Test]
    public void Should_extract_description_with_text_over_150_char_with_modules_in_between()
    {
        var document = TemplateParser.Run(
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"105,87\"}]]\r\n" +
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry.\r\n\r\n" +
            "[[{\"TemplateName\":\"Cards\", \"CardOrientation\":\"Landscape\", \"SetListIds\":\"109,87\"}]]\r\n" +
            "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
            new Category());

        var description = Document.GetDescription(document);

        Assert.That(document.Elements[0].Type, Is.EqualTo("cards"));
        Assert.That(document.Elements.Count(e => e.IsText), Is.EqualTo(2));
        Assert.That(description, Is.EqualTo("Lorem Ipsum is simply dummy text of the printing and typesetting industry.\r\nLorem Ipsum has been the industry's standard dummy text ever since the 150..."));
    }
}
