using System.Runtime.CompilerServices;

internal static class VerifyGlobalInit
{
    [ModuleInitializer]
    internal static void Init()
    {
        // Replace the values with «scrubbed» placeholders
        VerifierSettings.IgnoreMember("DateCreated");
        VerifierSettings.IgnoreMember("DateModified");
        VerifierSettings.IgnoreMember("CorrectnessProbability");
        VerifierSettings.IgnoreMember("QuestionChangeCacheItems");
        VerifierSettings.IgnoreMember("PageChangeCacheItems");

        VerifierSettings.IgnoreMember<PageRelationCache>(relation => relation.Child);
        VerifierSettings.IgnoreMember<PageRelationCache>(relation => relation.Parent);

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
        VerifierSettings.ScrubMember<PageCacheItem>(page => page.Creator);
        VerifierSettings.IgnoreMember("IsHistoric");
    }
}