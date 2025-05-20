using System.Runtime.CompilerServices;

internal static class VerifyGlobalInit
{
    [ModuleInitializer]
    internal static void Init()
    {
        // Replace the values with «scrubbed» placeholders
        VerifierSettings.ScrubMember("DateCreated");
        VerifierSettings.ScrubMember("DateModified");
        VerifierSettings.ScrubMember("CorrectnessProbability");
        VerifierSettings.ScrubMember("QuestionChangeCacheItems");
        VerifierSettings.ScrubMember("PageChangeCacheItems");

    }
}