﻿using Markdig;

public class MarkdownMarkdig
{
    public static string ToHtml(string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(markdown, pipeline);
    }
}