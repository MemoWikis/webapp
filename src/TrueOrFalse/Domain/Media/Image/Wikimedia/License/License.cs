using System.Collections.Generic;
using NHibernate.Mapping;

public class License
{
    public int Id;
    public string Name;
    /// <summary>
    /// Page where the license can be found online
    /// </summary>
    public string Url;

    public bool AuthorRequired;
    public bool CopyOfLicenseTextRequired;
    public string CopyOfLicenseTextUrl;

    public bool IsCCLicense;

    public List<string> WikipediaTemplateNames;
   
}