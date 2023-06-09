using System.Collections.Generic;
using System.Linq;

public class WikiLanguage
{
    public string LanguageToken;
    public string LanguageName;

    public WikiLanguage(string languageToken, string languageName)
    {
        LanguageToken = languageToken;
        LanguageName = languageName;
    }

    public static List<WikiLanguage> GetAllLanguages()
    {
        var tokenList = new List<string>
        {
            "aa", "ab", "af", "ak", "als", "am", "an", "ang", "ar", "arc", "as", "ast", "av", "ay", "az", "ba", "be", "be-tarask", "bg", "bh", "bi", "bm", "bn", "bo", "br", "bs", "ca", "ce", "ceb", "ch", "cho", "chr", "chy", "co", "cr", "cs", "csb", "cv", "cy", "da", "de", "dv", "dz", "el", "en", "eo", "es", "et", "eu", "fa", "ff", "fi", "fil", "fiu-vro", "fj", "fo", "fr", "fur", "fy", "ga", "gd", "gl", "gn", "got", "gu", "gv", "ha", "haw", "he", "hi", "ho", "hr", "ht", "hu", "hy", "hz", "ia", "id", "ie", "ig", "ii", "ik", "io", "is", "it", "iu", "ja", "jbo", "jv", "ka", "kg", "ki", "kj", "kk", "kl", "km", "kn", "ko", "kr", "ks", "ku", "kv", "kw", "ky", "la", "lad", "lb", "lez", "lg", "li", "ln", "lo", "lt", "lv", "mg", "mh", "mi", "mk", "ml", "mn", "mo", "mr", "ms", "mt", "mus", "mwl", "my", "mzn", "na", "nah", "nap", "nb", "nds", "nds-nl", "ne", "ng", "nl", "nn", "no", "nrm", "nv", "ny", "oc", "om", "or", "os", "pa", "pam", "pdt", "pi", "pl", "ps", "pt", "qu", "rm", "rn", "ro", "ru", "rup", "rw", "sa", "sc", "scn", "sco", "sd", "se", "sg", "sh", "si", "sk", "sl", "sm", "sn", "so", "sq", "sr", "ss", "st", "su", "sv", "sw", "ta", "te", "tg", "tgl", "tglg", "th", "ti", "tk", "tn", "to", "tokipona", "tpi", "tr", "ts", "tt", "tum", "tw", "ty", "ug", "uk", "ur", "uz", "ve", "vec", "vi", "vo", "wa", "war", "wo", "xh", "yi", "yo", "za", "zh", "zh-min-nan", "zu"
        };

        var languageNames = new List<string>
        {
            "Afar", "Abkhazian", "Afrikaans", "Akan", "Alemannisch", "Amharic", "Aragonese", "Old English", "Arabic", "Aramaic", "Assamese", "Asturian", "Avaric", "Aymar aru", "Azerbaijani", "Bashkir", "Belarusian", "Belarusian (Taraškievica orthography)", "български", "भोजपुरी", "Bislama", "Bambara", "Bengali", "Tibetan", "Breton", "Bosnian", "Catalan", "Chechen", "Cebuano", "Chamorro", "Choctaw", "Cherokee", "Cheyenne", "Corsican", "Cree", "Czech", "Kashubian", "Chuvash", "Welsh", "Danish", "German", "Divehi", "Dzongkha", "Greek", "English", "Esperanto", "Spanish", "Estonian", "Basque", "Persian", "Fulah", "Finnish", "Filipino", "Võro", "Fijian", "Faroese", "French", "Friulian", "Western Frisian", "Irish", "Scottish Gaelic", "Galician", "Guarani", "Gothic", "Gujarati", "Manx", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hiri Motu", "Croatian", "Haitian", "Hungarian", "Armenian", "Herero", "Interlingua", "Indonesian", "Interlingue", "Igbo", "Sichuan Yi", "Inupiaq", "Ido", "Icelandic", "Italian", "Inuktitut", "Japanese", "Lojban", "Basa Jawa", "Georgian", "Kongo", "Kikuyu", "Kuanyama", "Kazakh", "Kalaallisut", "Khmer", "Kannada", "Korean", "Kanuri", "Kashmiri", "Kurdish", "Komi", "Cornish", "Kyrgyz", "Latin", "Ladino", "Luxembourgish", "Lezghian", "Ganda", "Limburgish", "Lingala", "Lao", "Lithuanian", "Latvian", "Malagasy", "Marshallese", "Maori", "Macedonian", "Malayalam", "Mongolian", "молдовеняскэ", "Marathi", "Malay", "Maltese", "Creek", "Mirandese", "Burmese", "Mazanderani", "Nauru", "Nāhuatl", "Neapolitan", "Norwegian Bokmål", "Low German", "Nedersaksies", "Nepali", "Ndonga", "Dutch", "Norwegian Nynorsk", "Norwegian (bokmål)", "Nouormand", "Navajo", "Nyanja", "Occitan", "Oromo", "Oriya", "Ossetic", "Punjabi", "Pampanga", "Plautdietsch", "Pali", "Polish", "Pashto", "Portuguese", "Quechua", "rumantsch", "Rundi", "Romanian", "Russian", "Aromanian", "Kinyarwanda", "Sanskrit", "Sardinian", "Sicilian", "Scots", "Sindhi", "sámegiella", "Sango", "Serbo-Croatian", "Sinhala", "Slovak", "Slovenian", "Samoan", "Shona", "Somali", "Albanian", "Serbian", "Swati", "Southern Sotho", "Sundanese", "Swedish", "Kiswahili", "Tamil", "Telugu", "Tajik", "Tagalog", "tglg", "Thai", "Tigrinya", "Turkmen", "Tswana", "Tongan", "Toki Pona", "Tok Pisin", "Turkish", "Xitsonga", "Tatar", "Tumbuka", "Twi", "Tahitian", "Uyghur", "Ukrainian", "Urdu", "Uzbek", "Venda", "Venetian", "Vietnamese", "Volapük", "Walloon", "Winaray", "Wolof", "isiXhosa", "Yiddish", "Yoruba", "Zhuang", "Chinese", "Chinese (Min Nan)", "Zulu"
        };

        var allLanguages = new List<WikiLanguage>();
        int i;
        for (i = 0; i < tokenList.Count; i++)
            allLanguages.Add(new WikiLanguage(tokenList[i], languageNames[i]));
        

        return allLanguages;
    }
}