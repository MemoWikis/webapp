using System.Collections.Generic;
using System.Linq;

public class LicenseQuestionRepo
{
    public const int DefaultLicenseId = 1;

    public static List<LicenseQuestion> GetAllRegisteredLicenses()
    {
        return new List<LicenseQuestion>()
        {
            //Don't change IDs!
           
            new LicenseQuestion
            {
                Id = 1,
                NameLong = "Creative Commons: Namensnennung 4.0 International",
                NameShort = "CC BY 4.0",
                LicenseLinkOptional = @"https://creativecommons.org/licenses/by/4.0/legalcode",
                LicenseShortDescriptionLinkOptional = @"https://creativecommons.org/licenses/by/4.0/deed.de",
                AuthorRequired = true,
                LicenseLinkRequired = true,
                ChangesNotAllowed = false
            },

            new LicenseQuestion
            {
                Id = 2,
                NameLong = "Amtliches Werk im Sinne von § 5 Abs. 2 Urheberrechtsgesetz",
                NameShort = "Amtliches Werk",
                DisplayTextHtml = "Dieser Beitrag stellt ein \"andere[s] amtliche[s] Werk[]\" gemäß § 5 Abs. 2 des Urhererrechtsgesetzes dar.",
                AuthorRequired = true,
                LicenseLinkRequired = false,
                ChangesNotAllowed = true
            }
        };

    }

    public static LicenseQuestion GetById(int id)
    {
        return GetAllRegisteredLicenses().FirstOrDefault(license => license.Id == id);
    }

    public static LicenseQuestion GetDefaultLicense()
    {
        return GetAllRegisteredLicenses().Single(l => l.IsDefault());
    }
}

