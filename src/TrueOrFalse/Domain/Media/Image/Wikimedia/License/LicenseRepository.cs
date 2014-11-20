using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using EasyNetQ.Events;
using FluentNHibernate.Utils;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using SolrNet.Utils;
using TrueOrFalse;
using TrueOrFalse.WikiMarkup;

public class LicenseRepository
{
    public static List<License> GetAllRegisteredLicenses()
    {
        var registeredLicenses = new List<License>()
        { 
            //Don't change IDs!
            //Only set "LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded" if all necessary information is provided
            //(Id, WikiSearchString, LicenseRequirementsType or AuthorRequired/LicenseLinkRequired/CopyOfLicenseTextRequired, LicenseLink/CopyOfLicenseTextUrl if required)
            //Run Tests "Authorized_licenses_should_contain_all_necessary_information()" and "Registered_licenses_should_not_contain_duplicates()"
            //Find template on Wikimedia "http://commons.wikimedia.org/wiki/template:" + WikiSearchString + "?uselang=de"
            //Overview of all wikimedia licenses: https://commons.wikimedia.org/wiki/Commons:Image_copyright_tags_visual
            
            new License()
            {
                Id = 1,
                WikiSearchString = "cc-by-1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/1.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/1.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 1.0 Generic",
                LicenseShortName = "CC BY 1.0",
            },

            new License()
            {
                Id = 2,
                WikiSearchString = "cc-by-2.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/2.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/2.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 2.0 Generic",
                LicenseShortName = "CC BY 2.0",
            },

            new License()
            {
                Id = 3,
                WikiSearchString = "cc-by-2.5",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/2.5/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/2.5/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 2.5 Generic",
                LicenseShortName = "CC BY 2.5",
            },

            new License()
            {
                Id = 4,
                WikiSearchString = "cc-by-3.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 3.0 Unported",
                LicenseShortName = "CC BY 3.0",
            },

            new License()
            {
                Id = 5,
                WikiSearchString = "cc-by-3.0,2.5,2.0,1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 3.0 Unported",
                LicenseShortName = "CC BY 3.0",
            },

            new License()
            {
                Id = 6,
                WikiSearchString = "cc-sa-1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_Sa,
                LicenseLink = "http://creativecommons.org/licenses/sa/1.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/sa/1.0/deed.de",
                LicenseLongName = "Creative Commons: Weitergabe unter gleichen Bedingungen 1.0 Generic",
                LicenseShortName = "CC SA 1.0",
            },

            new License()
            {
                Id = 7,
                WikiSearchString = "cc-by-sa-1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/1.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/1.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 1.0 Generic",
                LicenseShortName = "CC BY-SA 1.0",
            },

            new License()
            {
                Id = 8,
                WikiSearchString = "cc-by-sa-2.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/2.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/2.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 2.0 Generic",
                LicenseShortName = "CC BY-SA 2.0",
            },

            new License()
            {
                Id = 9,
                WikiSearchString = "cc-by-sa-2.5",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/2.5/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/2.5/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 2.5 Generic",
                LicenseShortName = "CC BY-SA 2.5",
            },

            new License()
            {
                Id = 10,
                WikiSearchString = "cc-by-sa-3.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName = "Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },
            
            new License()
            {
                Id = 11,
                WikiSearchString = "cc-by-sa-3.0,2.5,2.0,1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },

            //Template for CC-BY-SA licenses:
            //new License()
            //{
            //    Id = 2,
            //    WikiSearchString = "cc-by-sa-3.0,2.5,2.0,1.0",
            //    LicenseApplicability = LicenseApplicability.,//Requirements should be recorded under License > InitLicenseSettings()
                
            //    LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
            //    LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",
                
            //    LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
            //    LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
            //    LicenseShortName = "CC BY-SA 3.0",
            //},

            //Template general:
            //new License()
            //{
            //    Id = ,
            //    WikiSearchString = "",
             
            //    Choose RequirementsType or add requirements manually
            //    LicenseRequirementsType = LicenseRequirementsType.,
            //        //LicenseLink = ,
            //        //CopyOfLicenseTextUrl = ,
            //    //or:
            //        AuthorRequired = ,
            //        LicenseLinkRequired = ,
            //        //LicenseLink = ,
            //        CopyOfLicenseTextRequired = ,
            //        //CopyOfLicenseTextUrl = ,

            //    LicenseApplicability = ,
            //}
        };

        registeredLicenses.ForEach(license => license.InitLicenseSettings());

        return registeredLicenses;

    }

    public static List<License> GetAllAuthorizedLicenses()
    {
        return GetAllRegisteredLicenses().Where(license => license.LicenseApplicability == LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded).ToList();
        //$temp: What about "LicenseApplicability.LicenseIsConditionallyApplicable"?
    }

    public static License GetById(int id)
    {
        return GetAllRegisteredLicenses().First(license => license.Id == id);
    }
}

public class LicenseParser
{
    public static List<License> GetAllParsedLicenses(string wikiMarkup)
    {
        return LicenseRepository.GetAllRegisteredLicenses()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup).Any(x => !String.IsNullOrEmpty(license.WikiSearchString)
                                                        && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static List<License> GetAuthorizedParsedLicenses(string wikiMarkup)
    {
        return LicenseRepository.GetAllAuthorizedLicenses()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup).Any(x => !String.IsNullOrEmpty(license.WikiSearchString) 
                                                        && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static List<License> GetAuthorizedParsedLicenses(List<License> allLicenses)
    {
        return SortLicenses(LicenseRepository.GetAllAuthorizedLicenses()
                                .Where(license => allLicenses.Any(x => x.Id == license.Id))
                                .ToList());
    }

    public static IList<License> GetNonAuthorizedParsedLicenses(string wikiMarkup)
    {
        return GetAllParsedLicenses(wikiMarkup).Except(GetAuthorizedParsedLicenses(wikiMarkup)).ToList();
    }
    
    public static License GetMainLicense(string wikiMarkup)
    {
        return GetAuthorizedParsedLicenses(wikiMarkup)
                .Where(license => CheckLicenseRequirements(license, wikiMarkup).AllRequirementsMet)
                .ToList()
                .FirstOrDefault();
    }

    public static int GetMainLicenseId(string wikiMarkup)
    {
        return GetMainLicense(wikiMarkup) != null ? GetMainLicense(wikiMarkup).Id : -1;
    }

    public static List<License> SortLicenses(List<License> licenseList)
    {
        return licenseList
            .OrderBy(license => license.LicenseRequirementsType.GetRank())
            .ThenByDescending(license => new GetCcLicenseComponents(license).CcVersion)
            .ThenBy(PriotizeByCcJurisdictionToken)
            .ThenBy(license => String.IsNullOrEmpty(license.WikiSearchString))//To have empty strings/null at the end
            .ThenBy(license => license.WikiSearchString)
            .ToList();
    }

    public static LicenseNotifications CheckLicenseRequirements(License license, string wikiMarkup)
    {
        var licenseNotifications = new LicenseNotifications();
        if (license.AuthorRequired.IsTrue() && String.IsNullOrEmpty(ParseImageMarkup.Run(wikiMarkup).AuthorName))
        {
            licenseNotifications.AuthorIsMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        if (license.LicenseLinkRequired.IsTrue() && String.IsNullOrEmpty(license.LicenseLink))
        {
            licenseNotifications.LicenseLinkIsMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        if (license.CopyOfLicenseTextRequired.IsTrue() && String.IsNullOrEmpty(license.CopyOfLicenseTextUrl))
        {
            licenseNotifications.LocalCopyOfLicenseUrlMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        return licenseNotifications;
    }

    public static ImageLicenseState CheckImageLicenseState(License license, string wikiMarkup)
    {
        if (LicenseRepository.GetAllRegisteredLicenses().Any(l => l.Id == license.Id))
        {
            if (LicenseRepository.GetAllAuthorizedLicenses().Any(l => l.Id == license.Id))
            {
                return CheckLicenseRequirements(license, wikiMarkup).AllRequirementsMet ? ImageLicenseState.LicenseIsApplicableForImage : ImageLicenseState.LicenseAuthorizedButInfoMissing;
            }
            return ImageLicenseState.LicenseIsNotAuthorized;
        }
        return ImageLicenseState.NotSpecified;
    }

    public static string GetImageLicenseStateMessage(License license, string wikiMarkup)
    {
        switch (CheckImageLicenseState(license, wikiMarkup))
        {
            case ImageLicenseState.LicenseIsApplicableForImage:
                return "verwendbar";

            case ImageLicenseState.LicenseAuthorizedButInfoMissing:
                return "zugelassen, aber benötigte Angaben unvollständig";

            case ImageLicenseState.LicenseIsNotAuthorized:
                return "nicht zugelassen";
        }

        return "unbekannt";
    }

    public static int PriotizeByCcJurisdictionToken(License license)    
    {
        var licenseComponents = new GetCcLicenseComponents(license);

        if (licenseComponents.CcJurisdictionPortsToken == "")
            return 1;
        if (licenseComponents.CcJurisdictionPortsToken == "de")
            return 2;
        return 99;
    }

    public static List<string> LicenseRegexSearchExpressions()
    {
        //$todo: refine
        return new List<string> { "^cc-", "^pd-", "^gfdl" };
    }

    public static List<string> GetOtherPossibleLicenseStrings(string wikiMarkup)
    {
        return ParseTemplate.TokenizeMarkup(wikiMarkup)
            .Where(token => LicenseRegexSearchExpressions()
            .Any(expression => Regex.Match(token, expression, RegexOptions.IgnoreCase).Success))
            .Except(GetAllParsedLicenses(wikiMarkup).Select(license => license.WikiSearchString))
            .ToList();
    }

    public static string GetWikiDetailsPageFromSourceUrl(string sourceUrl)
    {
        return !String.IsNullOrEmpty(sourceUrl) && sourceUrl.StartsWith("http://upload.wikimedia.org")
            ? "http://commons.wikimedia.org/wiki/File:" + Regex.Split(sourceUrl, "/").Last()
            : "";
    }
}

public class LicenseNotifications
{
    public bool AllRequirementsMet = true;
    public bool AuthorIsMissing;
    public bool LicenseLinkIsMissing;
    public bool LocalCopyOfLicenseUrlMissing;
}

public enum ImageLicenseState
{
    NotSpecified = 0,
    LicenseIsApplicableForImage = 1,
    LicenseAuthorizedButInfoMissing = 2,
    LicenseIsNotAuthorized = 3,

}

