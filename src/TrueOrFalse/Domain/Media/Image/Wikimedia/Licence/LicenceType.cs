using System.Collections.Generic;
using NHibernate.Mapping;

public class Licence
{
    public int Id;
    public string Name;
    /// <summary>
    /// Page where the licence can be found online
    /// </summary>
    public string Url;

    public bool AuthorRequired;
    public bool CopyOfLicenceTextRequired;
    public string CopyOfLicenceTextUrl;

    public List<string> WikipediaTemplateNames;
   
    public L
}