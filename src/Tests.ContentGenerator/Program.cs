using System.CommandLine;
using System.Text.Json;

/// <summary>
/// CLI tool to generate wiki content using AI and save to JSON files.
/// </summary>
class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Content Generator for memoWikis test scenarios");

        // Generate command
        var generateCommand = new Command("generate", "Generate new wiki content using AI");
        var themeOption = new Option<string>(
            name: "--theme",
            description: "Theme for the content (politics, history, technology, administration)");
        themeOption.IsRequired = true;

        var promptOption = new Option<string>(
            name: "--prompt",
            description: "Topic description for AI generation");
        promptOption.IsRequired = true;

        var countOption = new Option<int>(
            name: "--count",
            getDefaultValue: () => 1,
            description: "Number of wikis to generate");

        var difficultyOption = new Option<int>(
            name: "--difficulty",
            getDefaultValue: () => 3,
            description: "Difficulty level (1=ELI5, 2=Beginner, 3=Intermediate, 4=Advanced, 5=Academic)");

        generateCommand.AddOption(themeOption);
        generateCommand.AddOption(promptOption);
        generateCommand.AddOption(countOption);
        generateCommand.AddOption(difficultyOption);

        generateCommand.SetHandler(async (theme, prompt, count, difficulty) =>
        {
            await GenerateContent(theme, prompt, count, difficulty);
        }, themeOption, promptOption, countOption, difficultyOption);

        // List command
        var listCommand = new Command("list", "List all available content");
        var listThemeOption = new Option<string>(
            name: "--theme",
            getDefaultValue: () => "all",
            description: "Filter by theme (or 'all' for everything)");
        listCommand.AddOption(listThemeOption);

        listCommand.SetHandler((theme) =>
        {
            ListContent(theme);
        }, listThemeOption);

        rootCommand.AddCommand(generateCommand);
        rootCommand.AddCommand(listCommand);

        return await rootCommand.InvokeAsync(args);
    }

    static async Task GenerateContent(string theme, string prompt, int count, int difficulty)
    {
        Console.WriteLine($"🚀 Generating {count} wiki(s) for theme '{theme}'...");
        Console.WriteLine($"   Prompt: {prompt}");
        Console.WriteLine($"   Difficulty: {difficulty}");
        Console.WriteLine();

        // TODO: Initialize TestHarness and call AiPageGenerator
        Console.WriteLine("⚠️  AI generation not yet implemented.");
        Console.WriteLine("💡 You can manually create JSON files in:");
        Console.WriteLine($"   src/Tests/TestData.ScenarioBuilder/Content/{theme.ToLower()}/");
        Console.WriteLine();
        Console.WriteLine("📝 Example structure:");
        Console.WriteLine(GetExampleJson());
    }

    static void ListContent(string themeFilter)
    {
        var contentPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "..",
            "Tests", "TestData.ScenarioBuilder", "Content");

        if (!Directory.Exists(contentPath))
        {
            Console.WriteLine($"❌ Content directory not found: {contentPath}");
            return;
        }

        var themes = Directory.GetDirectories(contentPath)
            .Select(Path.GetFileName)
            .ToList();

        if (themeFilter != "all")
        {
            themes = themes.Where(t => t?.Equals(themeFilter, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }

        Console.WriteLine("📚 Available Content:");
        Console.WriteLine();

        foreach (var theme in themes)
        {
            if (theme == null) continue;

            var themePath = Path.Combine(contentPath, theme);
            var files = Directory.GetFiles(themePath, "*.json");

            Console.WriteLine($"🏷️  {theme.ToUpperInvariant()}");

            if (files.Length == 0)
            {
                Console.WriteLine("   (no content)");
            }
            else
            {
                foreach (var file in files)
                {
                    try
                    {
                        var json = File.ReadAllText(file);
                        var wiki = JsonSerializer.Deserialize<WikiContentDefinition>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (wiki != null)
                        {
                            Console.WriteLine($"   • {wiki.Title} ({wiki.Subpages.Count} subpages, {wiki.Questions.Count} questions)");
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"   • {Path.GetFileNameWithoutExtension(file)} (error loading)");
                    }
                }
            }
            Console.WriteLine();
        }
    }

    static string GetExampleJson()
    {
        return @"{
  ""title"": ""Your Wiki Title"",
  ""theme"": ""Politics"",
  ""difficulty"": 3,
  ""mainPage"": {
    ""title"": ""Main Page Title"",
    ""content"": ""<h2>Introduction</h2><p>Your content here...</p>""
  },
  ""subpages"": [
    {
      ""title"": ""Subpage Title"",
      ""content"": ""<h2>Details</h2><p>More content...</p>""
    }
  ],
  ""questions"": [
    {
      ""pageTitle"": ""Main Page Title"",
      ""text"": ""Your question?"",
      ""solution"": ""Your answer"",
      ""solutionType"": ""Text"",
      ""isCaseSensitive"": false
    }
  ],
  ""metadata"": {
    ""generatedAt"": ""2026-01-04T20:00:00Z"",
    ""aiModel"": ""manual"",
    ""sources"": []
  }
}";
    }
}
