public enum Language
{
    German,
    English,
    Spanish,
    French
}

public static class LanguageExtensions
{
    public static string? GetCode(this Language language)
    {
        return language switch
        {
            Language.German => "de",
            Language.English => "en",
            Language.Spanish => "es",
            Language.French => "fr",
            _ => null
        };
    }

    public static Language? GetLanguage(string code)
    {
        return code switch
        {
            "de" => Language.German,
            "en" => Language.English,
            "es" => Language.Spanish,
            "fr" => Language.French,
            _ => null
        };
    }

    public static bool CodeExists(string code)
    {
        return GetLanguage(code).HasValue;
    }

    public static void SetContentLanguageOnAuthors(int id) => SetContentLanguageOnAuthors(EntityCache.GetPage(id) ?? throw new InvalidOperationException());
    public static void SetContentLanguageOnAuthors(PageCacheItem page)
    {
        foreach (var authorId in page.AuthorIds)
        {
            var author = EntityCache.GetUserByIdNullable(authorId);
            if (author == null)
                continue;

            AddContentLanguageToUser(author, page.Language);
        }
    }

    public static void AddContentLanguageToUser(UserCacheItem user, string code)
    {
        var pageLanguage = GetLanguage(code);

        if (pageLanguage != null && user.ContentLanguages.All(language => language != pageLanguage))
            user.ContentLanguages.Add((Language)pageLanguage);
        else
            Logg.r.Error($"Could not set content language {code} on author {user.Id}");
    }

    public static void RemoveContentLanguageFromUser(UserCacheItem user, string code)
    {
        var pageLanguage = GetLanguage(code);

        if (pageLanguage != null && user.ContentLanguages.Any(language => language == pageLanguage))
            user.ContentLanguages.Remove((Language)pageLanguage);
        else
            Logg.r.Error($"Could not set content language {code} on author {user.Id}");
    }

    public static List<Language> GetLanguages()
    {
        return Enum.GetValues<Language>().ToList();
    }

    public static List<Language> GetLanguages(string[] codes)
    {
        return codes
            .Select(GetLanguage)
            .Where(language => language.HasValue)
            .Select(language => language.Value)
            .ToList();
    }

    public static bool LanguageIsInList(List<Language> languages, string code)
    {
        var language = GetLanguage(code);
        return language.HasValue && languages.Contains(language.Value);
    }

    public static bool AnyLanguageIsInList(List<Language> languages, string[] codes)
    {
        return codes.Any(code =>
        {
            var language = GetLanguage(code);
            return language.HasValue && languages.Contains(language.Value);
        });
    }

    public static bool AnyLanguageIsInList(List<Language> baseList, List<Language> languages)
    {
        return languages.Any(baseList.Contains);
    }
}
