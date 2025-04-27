public class ShouldReasons
{
    private static Dictionary<string, string> _shouldReasons = new();

    static ShouldReasons()
    {
        _shouldReasons.Add("shouldBePrivate", "Die Frage sollte privat sein.");
        _shouldReasons.Add("sourcesAreWrong", "Die Quellen sind nicht korrekt.");
        _shouldReasons.Add("answerNotClear", "Die Antwort ist falsch oder nicht eindeutig.");
        _shouldReasons.Add("improveOtherReason", "... ein anderer Grund.");

        _shouldReasons.Add("deleteIsOffending",
            "Die Frage ist beleidigend, abwertend oder rassistisch.");
        _shouldReasons.Add("deleteCopyright", "Urheberrechte werden verletzt.");
        _shouldReasons.Add("deleteIsSpam", "Es handelt sich um Spam.");
        _shouldReasons.Add("deleteOther", "... ein anderer Grund.");
    }

    public static string ByKey(string key)
    {
        return _shouldReasons[key];
    }

    public static List<string> ByKeys(string keysCsv)
    {
        if (String.IsNullOrEmpty(keysCsv))
            return new List<string>();

        return keysCsv.Split(',').Select(x => x.Trim()).Select(ByKey).ToList();
    }
}