using System.Collections.Generic;
using NHibernate.Mapping;

public class License
{
    public int Id;
    public string Name;
    public string SearchString;
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

   
}