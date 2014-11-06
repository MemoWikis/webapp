using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NHibernate.Mapping;

public class License
{
    public int Id;
    public string WikiSearchString;
    public string LicenseLongName;
    public string LicenseShortName;

    public LicenseApplicability LicenseApplicability;

    public bool? AuthorRequired;
    public bool? LicenseLinkRequired;
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

       //$Todo: complete


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
}

public class GetLicenseComponents
{
    public string LicenseGroup;
    public string CcJurisdictionPortsToken;
    public string CcVersion;

    public GetLicenseComponents(License license)
    {
        if (String.IsNullOrEmpty(license.WikiSearchString)) return;

        if (license.WikiSearchString.ToLower().StartsWith("cc-"))
        {
            LicenseGroup = "CC";
            CcVersion = Regex.Match(license.WikiSearchString, @"(?<=cc-((\w){2}-){1,2})(\d)(\.)(\d)\b", RegexOptions.IgnoreCase).Value;
            CcJurisdictionPortsToken = Regex.Match(license.WikiSearchString, @"(?<=cc-((\w){2}-){1,2}(\d)(\.)(\d)-)(\w){2}\b", RegexOptions.IgnoreCase).Value.ToLower();
        }

        else if (license.WikiSearchString.ToLower().StartsWith("pd"))
            LicenseGroup = "PD";

        else if (license.WikiSearchString.ToLower().StartsWith("gfdl"))
            LicenseGroup = "GFDL";
    }
}

public enum LicenseRequirementsType
{
    //Order by priority (order can be changed, not written to db), add requirements to InitLicenseSettings()  (and maybe identifier method to ParseLicenseRequirementsType):
    
    NoCategory = 0, //Rank: 999

    Cc_By = 1,
    Cc_By_Sa = 2,
    Cc_Sa = 3,//License is retired
    Cc0 = 4,
    PD = 5,
    GFDL = 6,

}

public static class LicenseRequirementsTypeExts
{
    public static int GetIntValue(this LicenseRequirementsType e)
    {
        return (int)Enum.Parse(typeof(LicenseRequirementsType), Enum.GetName(typeof(LicenseRequirementsType), e));
    }

    public static int GetRank(this LicenseRequirementsType e)
    {
        return e.GetIntValue() == 0 ? 999 : e.GetRank();
    }

}

