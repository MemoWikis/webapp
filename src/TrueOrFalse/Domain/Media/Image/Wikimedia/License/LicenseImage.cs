using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Seedworks.Lib;

public class LicenseImage
{
    public int Id { get; set; }
    public string WikiSearchString { get; set; }
    public string LicenseLongName;
    public string LicenseShortName;

    public LicenseApplicability LicenseApplicability;

    public bool? AuthorRequired;
    public bool? LicenseLinkRequired;
    public bool? ChangesNotAllowed;
    /// <summary>
    /// Page where the license can be found online
    /// </summary>
    public string LicenseLink;
    public string LicenseShortDescriptionLink;
    public bool? CopyOfLicenseTextRequired;
    public string CopyOfLicenseTextUrl;

    public LicenseRequirementsType LicenseRequirementsType;
    
    public List<string> WikipediaTemplateNames;

    public void InitLicenseSettings()
    {
        //Init requirements settings
        if (LicenseRequirementsType == LicenseRequirementsType.Cc_By_Sa)
        {
            AuthorRequired = true;
            LicenseLinkRequired = true;
            CopyOfLicenseTextRequired = false;
        }

        else if (LicenseRequirementsType == LicenseRequirementsType.Cc_By)
        {
            AuthorRequired = true;
            LicenseLinkRequired = true;
            CopyOfLicenseTextRequired = false;
        }

        else if (LicenseRequirementsType == LicenseRequirementsType.Cc_Sa)
        {
            AuthorRequired = false;
            LicenseLinkRequired = true;
            CopyOfLicenseTextRequired = false;
        }

        else if (LicenseRequirementsType == LicenseRequirementsType.GPL)//https://commons.wikimedia.org/wiki/Commons:Reusing_content_outside_Wikimedia/licenses#GNU_GPL_and_LGPL
        {
            AuthorRequired = true;
            LicenseLinkRequired = true;
            CopyOfLicenseTextRequired = false;
        }

        else if (LicenseRequirementsType == LicenseRequirementsType.GFDL)//http://commons.wikimedia.org/wiki/Commons:Weiterverwendung#GFDL
        {
            AuthorRequired = true;
            LicenseLinkRequired = true;
            CopyOfLicenseTextRequired = true;
        }

        else if (LicenseRequirementsType == LicenseRequirementsType.PD)//http://commons.wikimedia.org/wiki/Commons:Weiterverwendung#Rechtefreie_.28gemeinfreie.29_Inhalte
        {
            AuthorRequired = false;
            LicenseLinkRequired = false;
            CopyOfLicenseTextRequired = false;
        }
        else if (LicenseRequirementsType == LicenseRequirementsType.AmtlichesWerkDE)
        {
            AuthorRequired = true;
            LicenseLinkRequired = false;
            CopyOfLicenseTextRequired = false;
            ChangesNotAllowed = true;
        }
        else if (LicenseRequirementsType == LicenseRequirementsType.Cc0)
        {
            AuthorRequired = false;
            LicenseLinkRequired = false;
            CopyOfLicenseTextRequired = false;
            ChangesNotAllowed = false;
        }
        else if (LicenseRequirementsType == LicenseRequirementsType.MIT)
        {
            AuthorRequired = false;
            LicenseLinkRequired = false;
            CopyOfLicenseTextRequired = false;
            ChangesNotAllowed = false;
        }
        else if (LicenseRequirementsType == LicenseRequirementsType.NoCategory)
        {
            AuthorRequired = true;
            LicenseLinkRequired = true;
            CopyOfLicenseTextRequired = true;
            ChangesNotAllowed = true;
        }
    }

    public LicenseRequirementsType ParseLicenseRequirementsType()
    {
        if (String.IsNullOrEmpty(WikiSearchString))
            return LicenseRequirementsType.NoCategory;

        if (WikiSearchString.ToLower().StartsWith("cc-by-sa-")
            || WikiSearchString.ToLower().StartsWith("cc-by-"))
            return LicenseRequirementsType.Cc_By_Sa;

        if (WikiSearchString.ToLower().StartsWith("cc-sa-"))
            return LicenseRequirementsType.Cc_Sa;

        if (WikiSearchString.ToLower().StartsWith("cc-zero"))
            return LicenseRequirementsType.Cc0;

        if (WikiSearchString.ToLower().StartsWith("pd"))
            return LicenseRequirementsType.PD;

        if (WikiSearchString.ToLower().StartsWith("gfdl"))
            return LicenseRequirementsType.GFDL;

        return LicenseRequirementsType.NoCategory;
    }

    public static List<LicenseImage> FromLicenseIdList(string idList)
    {
        if (idList == null)
            return new List<LicenseImage>();

        int y;
        return Regex.Split(idList, ", ")
            .ToList()
            .Where(x => Int32.TryParse(x, out y))
            .Select(id => LicenseImageRepo.GetById(id.ToInt32()))
            .ToList();
    }

    public static string ToLicenseIdList(List<LicenseImage> licenses)
    {
        return string.Join(",", licenses.Select(x => x.Id.ToString()));
    } 
}

public class GetCcLicenseComponents
{
    public bool IsCC;
    public string CcJurisdictionPortsToken;
    public string CcVersion;

    public GetCcLicenseComponents(LicenseImage license)
    {
        if (String.IsNullOrEmpty(license.WikiSearchString)) return;

        if (license.WikiSearchString.ToLower().StartsWith("cc-") &&
            (license.LicenseRequirementsType == LicenseRequirementsType.Cc0 ||
             license.LicenseRequirementsType == LicenseRequirementsType.Cc_By ||
             license.LicenseRequirementsType == LicenseRequirementsType.Cc_By_Sa ||
             license.LicenseRequirementsType == LicenseRequirementsType.Cc_Sa))
        {
            IsCC = true;
            CcVersion = Regex.Match(license.WikiSearchString, "(?<=cc-([a-z]{2}-){1,2})\\d\\.\\d\\b", RegexOptions.IgnoreCase).Value;
            CcJurisdictionPortsToken = Regex.Match(license.WikiSearchString, "(?<=cc-([a-z]{2}-){1,2}\\d\\.\\d-)[a-z]{2,}\\b", RegexOptions.IgnoreCase).Value.ToLower();
        }
    }
}

public enum LicenseRequirementsType
{
    //Order by priority (order can be changed, not written to db, adjust test), add requirements to InitLicenseSettings() (and maybe identifier method to ParseLicenseRequirementsType):
    
    NoCategory = 0, //Rank: 999

    Cc_By = 1,
    Cc_By_Sa = 2,
    Cc_Sa = 3,//License is retired
    Cc0 = 4,
    PD = 5,
    GFDL = 6,
    AmtlichesWerkDE = 7,
    GPL = 8,
    MIT = 9
}

public static class LicenseRequirementsTypeExts
{
    public static int GetIntValue(this LicenseRequirementsType e)
    {
        return (int)Enum.Parse(typeof(LicenseRequirementsType), Enum.GetName(typeof(LicenseRequirementsType), e));
    }

    public static int GetRank(this LicenseRequirementsType e)
    {
        return e.GetIntValue() == 0 ? 999 : e.GetIntValue();
    }

}

