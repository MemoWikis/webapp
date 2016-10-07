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
                LicenseShortDescriptionLinkOptional = @"https://creativecommons.org/licenses/by/4.0/deed.de"
            },

            new LicenseQuestion
            {
                Id = 2,
                NameLong = "Andere Lizenz lang",
                NameShort = "Andere Lizenz kurz",
                DisplayTextHtml = "Dieser Beitrag steht unter Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua."
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

