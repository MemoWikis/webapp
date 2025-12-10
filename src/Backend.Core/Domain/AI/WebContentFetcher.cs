using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

public class WebContentFetcher : IRegisterAsInstancePerLifetime
{
    public record struct FetchedContent(string Title, string TextContent, string Url);

    public async Task<FetchedContent?> FetchAndExtract(string url)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                Log.Warning("Invalid URL provided: {Url}", url);
                return null;
            }

            // Create a new HttpClient per request with full browser-like headers
            using var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 5,
                AutomaticDecompression = System.Net.DecompressionMethods.All
            };
            
            using var httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Set comprehensive browser-like headers
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,de;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
            httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
            httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            var response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();
            return await ExtractContent(html, url);
        }
        catch (HttpRequestException exception)
        {
            Log.Error(exception, "Failed to fetch URL: {Url}", url);
            return null;
        }
        catch (TaskCanceledException exception)
        {
            Log.Error(exception, "Timeout fetching URL: {Url}", url);
            return null;
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Unexpected error fetching URL: {Url}", url);
            return null;
        }
    }

    private async Task<FetchedContent?> ExtractContent(string html, string url)
    {
        try
        {
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(html);

            // Remove unwanted elements
            var elementsToRemove = document.QuerySelectorAll("script, style, nav, footer, header, aside, noscript, iframe, svg, form, [role='navigation'], [role='banner'], [role='contentinfo']");
            foreach (var element in elementsToRemove)
            {
                element.Remove();
            }

            // Extract title
            var title = ExtractTitle(document);

            // Extract main content
            var textContent = ExtractMainContent(document);

            if (string.IsNullOrWhiteSpace(textContent))
            {
                Log.Warning("No content extracted from URL: {Url}", url);
                return null;
            }

            // Truncate if too long (Claude has token limits)
            const int maxCharacters = 50000;
            if (textContent.Length > maxCharacters)
            {
                textContent = textContent.Substring(0, maxCharacters) + "\n\n[Content truncated...]";
            }

            return new FetchedContent(title, textContent, url);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to extract content from HTML");
            return null;
        }
    }

    private string ExtractTitle(IDocument document)
    {
        // Try to get title from <title> tag
        var titleElement = document.QuerySelector("title");
        if (titleElement != null && !string.IsNullOrWhiteSpace(titleElement.TextContent))
        {
            return CleanText(titleElement.TextContent);
        }

        // Try to get from <h1>
        var h1Element = document.QuerySelector("h1");
        if (h1Element != null && !string.IsNullOrWhiteSpace(h1Element.TextContent))
        {
            return CleanText(h1Element.TextContent);
        }

        // Try to get from og:title meta tag
        var ogTitle = document.QuerySelector("meta[property='og:title']");
        if (ogTitle != null)
        {
            var content = ogTitle.GetAttribute("content");
            if (!string.IsNullOrWhiteSpace(content))
            {
                return CleanText(content);
            }
        }

        return "Imported Page";
    }

    private string ExtractMainContent(IDocument document)
    {
        // Try common content containers in order of preference
        var contentSelectors = new[]
        {
            "article",
            "main",
            "#content",
            "#main-content",
            "#mw-content-text", // Wikipedia
            ".markdown-body", // GitHub
            ".documentation",
            "[role='main']",
            ".content",
            ".post",
            ".article",
            "body"
        };

        foreach (var selector in contentSelectors)
        {
            var contentElement = document.QuerySelector(selector);
            if (contentElement != null)
            {
                var text = ExtractTextFromElement(contentElement);
                if (!string.IsNullOrWhiteSpace(text) && text.Length > 200)
                {
                    return text;
                }
            }
        }

        // Fallback: get all text from body
        var bodyElement = document.QuerySelector("body");
        return bodyElement != null ? ExtractTextFromElement(bodyElement) : "";
    }

    private string ExtractTextFromElement(IElement element)
    {
        var textBuilder = new System.Text.StringBuilder();
        ExtractTextRecursive(element, textBuilder);
        return CleanText(textBuilder.ToString());
    }

    private void ExtractTextRecursive(INode node, System.Text.StringBuilder textBuilder)
    {
        if (node.NodeType == NodeType.Text)
        {
            var text = node.TextContent;
            if (!string.IsNullOrWhiteSpace(text))
            {
                textBuilder.Append(text.Trim());
                textBuilder.Append(" ");
            }
            return;
        }

        if (node is IElement element)
        {
            // Add line breaks for block elements
            var blockElements = new[] { "p", "div", "br", "h1", "h2", "h3", "h4", "h5", "h6", "li", "tr" };
            var isBlock = blockElements.Contains(element.LocalName.ToLower());

            foreach (var child in node.ChildNodes)
            {
                ExtractTextRecursive(child, textBuilder);
            }

            if (isBlock)
            {
                textBuilder.AppendLine();
            }
        }
        else
        {
            foreach (var child in node.ChildNodes)
            {
                ExtractTextRecursive(child, textBuilder);
            }
        }
    }

    private string CleanText(string text)
    {
        // Remove extra whitespace
        text = Regex.Replace(text, @"\s+", " ");
        // Remove multiple newlines
        text = Regex.Replace(text, @"\n\s*\n+", "\n\n");
        return text.Trim();
    }
}
