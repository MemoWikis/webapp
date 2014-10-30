using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NHibernate.Mapping;

public class License
{
    public int Id;
    public string Name;
    public string WikiSearchString;
    public string LicenseDisplayName;
    /// <summary>
    /// Page where the license can be found online
    /// </summary>
    public string Url;

    public LicenseApplicability LicenseApplicability;

    public bool AuthorRequired;
    public bool CopyOfLicenseTextRequired;
    public string CopyOfLicenseTextUrl;

    public bool IsCC;
    public bool IsCC_BY_SA;
    public bool IsPD;//Also: CC0
    public bool IsGFDL;

    public List<string> WikipediaTemplateNames;

    public License() { }

    public License(bool isCC_BY_SA)
    {
        IsCC_BY_SA = isCC_BY_SA;
        
        if (isCC_BY_SA)
        {
            LicenseApplicability = LicenseApplicability.LicenseIsSafelyApplicable;
            IsCC = true;
            AuthorRequired = true;
        }
    }

    public LicenseCategory GetLicenseCategory()
    {
        if (String.IsNullOrEmpty(WikiSearchString))
            return LicenseCategory.NoCategory;

        if (WikiSearchString.ToLower().StartsWith("cc-by-sa-")
            || WikiSearchString.ToLower().StartsWith("cc-by-"))
            return LicenseCategory.Cc_By_Sa;

        if (WikiSearchString.ToLower().StartsWith("cc-sa-"))
            return LicenseCategory.Cc_Sa;

        if (WikiSearchString.ToLower().StartsWith("cc-zero"))
            return LicenseCategory.Cc0;

        if (WikiSearchString.ToLower().StartsWith("pd"))
            return LicenseCategory.PD;

        if (WikiSearchString.ToLower().StartsWith("gfdl"))
            return LicenseCategory.GFDL;

        return LicenseCategory.NoCategory;
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

        if (license.WikiSearchString.ToLower().StartsWith("pd"))
            LicenseGroup = "PD";

        if (license.WikiSearchString.ToLower().StartsWith("gfdl"))
        {
            LicenseGroup = "GFDL";
        }
    }
}

public enum LicenseCategory
{
    //Order by priority (order can be changed, not written to db), add identifier method to GetLicenseCategory:
    Cc_Sa = 0,
    Cc_By_Sa = 1,
    Cc0 = 2,
    PD = 3,
    GFDL = 4,

    NoCategory = 99
}

public static class LicenseCategoryExts
{
    public static int GetIntValue(this LicenseCategory e)
    {
        return (int)Enum.Parse(typeof(LicenseCategory), Enum.GetName(typeof(LicenseCategory), e));
    }
}

