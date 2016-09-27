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
                NameLong = "Creative Commons: Namensnennung 4.0 Unported",
                NameShort = "CC BY 4.0",
            },

            new LicenseQuestion
            {
                Id = 2,
                NameLong = "Andere Lizenz lang",
                NameShort = "Andere Lizenz kurz"
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

