using System.Runtime.CompilerServices;

internal static class VerifyGlobalInit
{
    [ModuleInitializer]
    internal static void Init()
    {
        // Disable auto-launching of diff tools only when NOT debugging in Visual Studio
        // This allows manual testing in Visual Studio to show diffs, while keeping automated tests clean
        if (!System.Diagnostics.Debugger.IsAttached)
        {
            Environment.SetEnvironmentVariable("DiffEngine_Disabled", "true");
        }
        
        // Replace the values with «scrubbed» placeholders
        VerifierSettings.IgnoreMember("DateCreated");
        VerifierSettings.IgnoreMember("DateModified");
        VerifierSettings.IgnoreMember("CorrectnessProbability");
        VerifierSettings.IgnoreMember("QuestionChangeCacheItems");
        VerifierSettings.IgnoreMember("PageChangeCacheItems");


        //Ignore irrelevant properties in Page
        VerifierSettings.IgnoreMember("TopicMarkdown");
        VerifierSettings.IgnoreMember("SkipMigration");
        VerifierSettings.IgnoreMember("TotalRelevancePersonalEntries");
        VerifierSettings.IgnoreMember("Url");
        VerifierSettings.IgnoreMember("UrlLinkText");
        VerifierSettings.IgnoreMember("WikipediaURL");
        VerifierSettings.IgnoreMember("DisableLearningFunctions");
        VerifierSettings.IgnoreMember("TextIsHidden");
        VerifierSettings.IgnoreMember("Description");
        VerifierSettings.IgnoreMember("CustomSegments");
        VerifierSettings.IgnoreMember("Content");
        VerifierSettings.IgnoreMember("CorrectnessProbabilityAnswerCount");
        VerifierSettings.IgnoreMember("CountQuestionsAggregated");
        VerifierSettings.IgnoreMember("AuthorIds");
        VerifierSettings.IgnoreMember("Language");
        VerifierSettings.IgnoreMember("IsHistoric");
        VerifierSettings.IgnoreMember("AuthorIdsInts");

        VerifierSettings.IgnoreMember<Page>(page => page.Creator);
        VerifierSettings.IgnoreMember<PageCacheItem>(page => page.Creator);


        //ignore irrelevant properties in Question
        VerifierSettings.IgnoreMember<QuestionCacheItem>(question => question.Creator);
        VerifierSettings.IgnoreMember<QuestionCacheItem>(question => question.Solution);
        VerifierSettings.IgnoreMember<QuestionCacheItem>(question => question.SolutionMetadataJson);
        VerifierSettings.IgnoreMember<QuestionCacheItem>(question => question.SolutionType);
        VerifierSettings.IgnoreMember<QuestionCacheItem>(question => question.Text);

        //page relations
        VerifierSettings.IgnoreMember<PageRelationCache>(relation => relation.Child);
        VerifierSettings.IgnoreMember<PageRelationCache>(relation => relation.Parent);

        // VerifierSettings.IgnoreMember<PageRelation>(relation => relation.Child);
        // VerifierSettings.IgnoreMember<PageRelation>(relation => relation.Parent);

        VerifierSettings.IgnoreMember("CollaborationToken");
    }
}