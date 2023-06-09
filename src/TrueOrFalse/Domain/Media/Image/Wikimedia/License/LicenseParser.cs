using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrueOrFalse.WikiMarkup;
using static System.String;

public class LicenseParser
{
    public static List<LicenseImage> GetAllParsedLicenses(string wikiMarkup)
    {
        return LicenseImageRepo.GetAllRegisteredLicenses()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup)
            .Any(x => !IsNullOrEmpty(license.WikiSearchString) && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static List<LicenseImage> GetAuthorizedParsedLicenses(string wikiMarkup)
    {
         return LicenseImageRepo.GetAllAuthorizedLicenses()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup)
            .Any(x => !IsNullOrEmpty(license.WikiSearchString)  && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static List<LicenseImage> GetAuthorizedParsedLicenses(List<LicenseImage> allLicenses)
    {
        return SortLicenses(
            LicenseImageRepo
                .GetAllAuthorizedLicenses()
                .Where(license => allLicenses.Any(x => x.Id == license.Id))
                .ToList()
        );
    }

    public static LicenseImage SuggestMainLicenseFromMarkup(ImageMetaData imageMetaData)
    {
        return GetAuthorizedParsedLicenses(imageMetaData.Markup)
                .Where(license => CheckLicenseRequirementsWithMarkup(license, imageMetaData).AllRequirementsMet)
                .ToList()
                .FirstOrDefault();
    }

    public static LicenseImage SuggestMainLicenseFromParsedList(ImageMetaData imageMetaData)
    {
        return GetAuthorizedParsedLicenses(LicenseImage.FromLicenseIdList(imageMetaData.AllRegisteredLicenses))
            .Where(license => CheckLicenseRequirementsWithDb(license, imageMetaData).AllRequirementsMet)
                .ToList()
                .FirstOrDefault();
    }

    public static List<LicenseImage> ParseAllRegisteredLicenses(string markup)
    {
        return SortLicenses(GetAllParsedLicenses(markup));
    } 

    public static List<LicenseImage> SortLicenses(List<LicenseImage> licenseList)
    {
        return licenseList
            .OrderBy(license => license.LicenseRequirementsType.GetRank())
            .ThenByDescending(license => new GetCcLicenseComponents(license).CcVersion)
            .ThenBy(PriotizeByCcJurisdictionToken)
            .ThenBy(license => IsNullOrEmpty(license.WikiSearchString))//To have empty strings/null at the end
            .ThenBy(license => license.WikiSearchString)
            .ToList();
    }

    public static LicenseNotifications CheckLicenseRequirementsWithMarkup(LicenseImage license, ImageMetaData imageMetaData)
    {
        return CheckLicenseRequirements(
            license,
            imageMetaData,
            !IsNullOrEmpty(ParseImageMarkup.Run(imageMetaData.Markup).AuthorName)
                ? ParseImageMarkup.Run(imageMetaData.Markup).AuthorName
                : "");
    }

    public static LicenseNotifications CheckLicenseRequirementsWithDb(LicenseImage license, ImageMetaData imageMetaData)
    {
        return CheckLicenseRequirements(
            license,
            imageMetaData,
            imageMetaData.AuthorParsed);
    }

    private static LicenseNotifications CheckLicenseRequirements(LicenseImage license, ImageMetaData imageMetaData, string author)
    {
        var licenseNotifications = new LicenseNotifications();
        if (license.AuthorRequired.IsTrue() &&
            IsNullOrEmpty(author) &&
            IsNullOrEmpty(imageMetaData.ManualEntriesFromJson().AuthorManuallyAdded))
        {
            licenseNotifications.AuthorIsMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        if (license.LicenseLinkRequired.IsTrue() && IsNullOrEmpty(license.LicenseLink))
        {
            licenseNotifications.LicenseLinkIsMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        if (license.CopyOfLicenseTextRequired.IsTrue() && IsNullOrEmpty(license.CopyOfLicenseTextUrl))
        {
            licenseNotifications.LocalCopyOfLicenseUrlMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        return licenseNotifications;
    }

    public static LicenseState CheckImageLicenseState(LicenseImage license, ImageMetaData imageMetaData)
    {
        if (LicenseImageRepo.GetAllRegisteredLicenses().Any(l => l.Id == license.Id))
        {
            if (LicenseImageRepo.GetAllAuthorizedLicenses().Any(l => l.Id == license.Id))
            {
                return CheckLicenseRequirementsWithDb(license, imageMetaData).AllRequirementsMet ?
                    LicenseState.IsApplicableForImage : 
                    LicenseState.AuthorizedButInfoMissing;
            }
            return LicenseState.IsNotAuthorized;
        }
        return LicenseState.NotSpecified;
    }

    public static int PriotizeByCcJurisdictionToken(LicenseImage license)    
    {
        var licenseComponents = new GetCcLicenseComponents(license);

        if (licenseComponents.CcJurisdictionPortsToken == "")
            return 1;

        if (licenseComponents.CcJurisdictionPortsToken == "de")
            return 2;

        return 99;
    }

    public static List<string> LicenseRegexSearchExpressions() => new List<string> { "^cc-", "^pd-", "^gfdl" };

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
        return !IsNullOrEmpty(sourceUrl) && 
            (sourceUrl.StartsWith("http://upload.wikimedia.org") || sourceUrl.StartsWith("https://upload.wikimedia.org"))
            ? "http://commons.wikimedia.org/wiki/File:" + Regex.Split(sourceUrl, "/").Last()
            : "";
    }
}