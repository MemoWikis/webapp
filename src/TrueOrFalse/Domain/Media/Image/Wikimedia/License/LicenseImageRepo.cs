public class LicenseImageRepo
{
    public static List<LicenseImage> GetAllRegisteredLicenses()
    {
        var registeredLicenses = new List<LicenseImage>
        {
            //Don't change IDs!
            //Only set "LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded" if all necessary information is provided
            //(Id, WikiSearchString, LicenseRequirementsType or AuthorRequired/LicenseLinkRequired/CopyOfLicenseTextRequired, LicenseLink/CopyOfLicenseTextUrl if required)
            //Run Tests "Authorized_licenses_should_contain_all_necessary_information()" and "Registered_licenses_should_not_contain_duplicates()"
            //Find template on Wikimedia "http://commons.wikimedia.org/wiki/template:" + WikiSearchString + "?uselang=de"
            //Overview of all wikimedia licenses: https://commons.wikimedia.org/wiki/Commons:Image_copyright_tags_visual

            //LicenseImage id=13 is not a Wikimedia-License, but treated as one here (e.g., contains a "WikiSearchString"). ImageLicenses should be treated seperately from Wikimedia,
            //to allow for other licenses not covered there.

            //to do: add link to author! Often, Wikipedia-Authors link to their profiles!
            new()
            {
                Id = 1,
                WikiSearchString = "cc-by-1.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/1.0/legalcode",

                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/1.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 1.0 Generic",
                LicenseShortName = "CC BY 1.0",
            },
            new()
            {
                Id = 2,
                WikiSearchString = "cc-by-2.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/2.0/legalcode",

                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/2.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 2.0 Generic",
                LicenseShortName = "CC BY 2.0",
            },
            new()
            {
                Id = 3,
                WikiSearchString = "cc-by-2.5",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/2.5/legalcode",

                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/2.5/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 2.5 Generic",
                LicenseShortName = "CC BY 2.5",
            },
            new()
            {
                Id = 4,
                WikiSearchString = "cc-by-3.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/3.0/legalcode",

                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 3.0 Unported",
                LicenseShortName = "CC BY 3.0",
            },
            new()
            {
                Id = 5,
                WikiSearchString = "cc-by-3.0,2.5,2.0,1.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/3.0/legalcode",

                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 3.0 Unported",
                LicenseShortName = "CC BY 3.0",
            },
            new()
            {
                Id = 6,
                WikiSearchString = "cc-sa-1.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_Sa,
                LicenseLink = "http://creativecommons.org/licenses/sa/1.0/legalcode",

                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/sa/1.0/deed.de",
                LicenseLongName =
                    "Creative Commons: Weitergabe unter gleichen Bedingungen 1.0 Generic",
                LicenseShortName = "CC SA 1.0",
            },
            new()
            {
                Id = 7,
                WikiSearchString = "cc-by-sa-1.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/1.0/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/1.0/deed.de",
                LicenseLongName =
                    "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 1.0 Generic",
                LicenseShortName = "CC BY-SA 1.0",
            },
            new()
            {
                Id = 8,
                WikiSearchString = "cc-by-sa-2.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/2.0/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/2.0/deed.de",
                LicenseLongName =
                    "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 2.0 Generic",
                LicenseShortName = "CC BY-SA 2.0",
            },
            new()
            {
                Id = 9,
                WikiSearchString = "cc-by-sa-2.5",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/2.5/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/2.5/deed.de",
                LicenseLongName =
                    "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 2.5 Generic",
                LicenseShortName = "CC BY-SA 2.5",
            },
            new()
            {
                Id = 10,
                WikiSearchString = "cc-by-sa-3.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName =
                    "Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },
            new()
            {
                Id = 11,
                WikiSearchString = "cc-by-sa-3.0,2.5,2.0,1.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName =
                    "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },
            new()
            {
                Id = 12,
                WikiSearchString = "cc-by-sa-4.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/4.0/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/4.0/deed.de",
                LicenseLongName =
                    "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 4.0 International",
                LicenseShortName = "CC BY-SA 4.0",
            },
            new()
            {
                //License requires stating the source and prohibits changes
                Id = 13,
                WikiSearchString = "Anderes Amtliches Werk",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.AmtlichesWerkDE,
                LicenseLink = "https://www.gesetze-im-internet.de/urhg/__5.html",

                LicenseShortDescriptionLink = "",
                LicenseLongName =
                    "Anderes Amtliches Werk im Sinne von � 5 Abs. 2 Urheberrechtsgesetz",
                LicenseShortName = "Amtliches Werk",
            },
            new()
            {
                Id = 14,
                WikiSearchString = "cc-by-4.0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "https://creativecommons.org/licenses/by/4.0/legalcode",

                LicenseShortDescriptionLink = "https://creativecommons.org/licenses/by/4.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 4.0 International",
                LicenseShortName = "CC BY 4.0",
            },
            new()
            {
                Id = 15,
                WikiSearchString = "cc-by-sa-3.0-de",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "https://creativecommons.org/licenses/by-sa/3.0/de/legalcode",

                LicenseShortDescriptionLink =
                    "https://creativecommons.org/licenses/by-sa/3.0/de/deed.de",
                LicenseLongName =
                    "Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Deutschland",
                LicenseShortName = "CC BY-SA 3.0 DE",
            },
            new()
            {
                Id = 16,
                WikiSearchString = "migration=relicense",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",

                LicenseShortDescriptionLink =
                    "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName =
                    "Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },
            new()
            {
                Id = 17,
                WikiSearchString = "Ag�ncia Brasil",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "https://creativecommons.org/licenses/by/3.0/br/legalcode",

                LicenseShortDescriptionLink =
                    "https://creativecommons.org/licenses/by/3.0/br/deed.pt",
                LicenseLongName = "Atribui��o 3.0 Brasil",
                LicenseShortName = "CC BY 3.0 BR",
            },
            new()
            {
                Id = 18,
                WikiSearchString = "CC0",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc0,
                LicenseLink = "https://creativecommons.org/publicdomain/zero/1.0/legalcode",

                LicenseShortDescriptionLink =
                    "https://creativecommons.org/publicdomain/zero/1.0/deed.de",
                LicenseLongName =
                    "Creative Commons Zero (Gemeinfreiheit, keine Rechte vorbehalten)",
                LicenseShortName = "CC0",
            },
            new()
            {
                Id = 19,
                WikiSearchString = "MIT_License",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.MIT,
                LicenseLink = "https://opensource.org/licenses/mit-license.php",

                LicenseShortDescriptionLink = "https://de.wikipedia.org/wiki/MIT-Lizenz",
                LicenseLongName = "MIT-Lizenz",
                LicenseShortName = "MIT",
            },
            new()
            {
                Id = 20,
                WikiSearchString = "Unsplash License",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc0,
                LicenseLink = "https://unsplash.com/license",

                LicenseShortDescriptionLink = "https://unsplash.com/license",
                LicenseLongName = "Unsplash license",
                LicenseShortName = "Unsplash",
            },
            new()
            {
                Id = 21,
                WikiSearchString = "GNU Lesser General Public License",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.NoPage,
                LicenseLink = "http://www.gnu.org/licenses/lgpl.html",
                CopyOfLicenseTextUrl = "https://www.gnu.org/licenses/gpl-3.0.txt",
                LicenseShortDescriptionLink = "http://www.gnu.org/licenses/lgpl.html",
                LicenseLongName = "GNU Lesser General Public License",
                LicenseShortName = "LGPL",
            },
            new()
            {
                Id = 198,
                WikiSearchString =
                    "Public Domain / Gemeinfrei (Amtliches Werk)", //Wiki-URL: https://de.wikipedia.org/wiki/Vorlage:Bild-PD-Amtliches_Werk
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                //LicenseLink = "https://www.gesetze-im-internet.de/urhg/__5.html", //valid only for Germany, but 
                LicenseRequirementsType = LicenseRequirementsType.PD
            },
            new()
            {
                Id = 199,
                WikiSearchString =
                    "Public Domain / Gemeinfrei", //Wiki-URL: https://de.wikipedia.org/wiki/Vorlage:Bild-PD-Amtliches_Werk
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD
            },
            new()
            {
                Id = 200,
                WikiSearchString = "pd-old",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD
            },
            new()
            {
                Id = 201,
                WikiSearchString = "PD-UA-exempt",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain (Ukraine) / Gemeinfrei"
            },
            new()
            {
                Id = 202,
                WikiSearchString = "PD-ineligible",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain"
            },
            new()
            {
                Id = 203,
                WikiSearchString = "PD-RU-exemp",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain"
            },
            new()
            {
                Id = 204,
                WikiSearchString = "PD-Art",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain"
            },
            new()
            {
                Id = 205,
                WikiSearchString = "GPL",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.GPL,
                LicenseLink = "https://www.gnu.org/copyleft/gpl-3.0.html",
            },
            new()
            {
                Id = 206,
                WikiSearchString = "PD-US",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain - published in USA before 1923"
            },
            new()
            {
                Id = 207,
                WikiSearchString = "PD-textlogo",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain - text logo",
                LicenseLink = "https://commons.wikimedia.org/wiki/Template_talk:PD-textlogo"
            },
            new()
            {
                Id = 208,
                WikiSearchString = "PD-user",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain - by user",
            },
            new()
            {
                Id = 209,
                WikiSearchString = "PD-user-w",
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Public domain - by user",
            },
            new()
            {
                Id = 555,
                LicenseApplicability =
                    LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                LicenseRequirementsType = LicenseRequirementsType.PD,
                LicenseLongName = "Youtube Preview Image",
            }
        };
        registeredLicenses.ForEach(license => license.InitLicenseSettings());
        return registeredLicenses;
    }

    public static List<LicenseImage> GetAllAuthorizedLicenses()
    {
        return GetAllRegisteredLicenses()
            .Where(license => license.LicenseApplicability ==
                              LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded)
            .ToList();

        //$temp: What about "LicenseApplicability.LicenseIsConditionallyApplicable"?
    }

    public static LicenseImage GetById(int id)
    {
        return GetAllRegisteredLicenses().FirstOrDefault(license => license.Id == id);
    }
}