using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using FluentNHibernate.Utils;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using TrueOrFalse;
using TrueOrFalse.WikiMarkup;

public class LicenseRepository
{
    public static List<License> GetAll()
    {
        return new List<License>()
        {
            //Don't change IDs!
            new License(true) {Id = 1, Name = "test", WikiSearchString = "cc-by-sa-3.0"},
            new License(true) {Id = 2, Name = "test", WikiSearchString = "cc-by-sa-3.0,2.5,2.0,1.0"},
            new License {Id = 991, Name = "CC", AuthorRequired = true},
            new License {Id = 992, Name = "KOREAN-STAMPS", AuthorRequired = true},
            //cc-by-sa-3.0,2.5,2.0,1.0
        };
    }

    

    public static List<License> GetAllApplicable()
    {
        return GetAll().Where(license => license.LicenseApplicability == LicenseApplicability.LicenseIsSafelyApplicable).ToList();
        //$temp: What about "LicenseApplicability.LicenseIsConditionallyApplicable"?
    }

    public static License GetById(int id)
    {
        return GetAll().First(license => license.Id != null && license.Id == id);
    }
}


public class LicenseParser
{
    public static IList<License> GetAllLicenses(string wikiMarkup)
    {
        return LicenseRepository.GetAll()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup).Any(x => !String.IsNullOrEmpty(license.WikiSearchString)
                                                        && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    } 

    public static IList<License> GetApplicableLicenses(string wikiMarkup)
    {
        return LicenseRepository.GetAllApplicable()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup).Any(x => !String.IsNullOrEmpty(license.WikiSearchString) 
                                                        && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static IList<License> GetNonApplicableLicenses(string wikiMarkup)
    {
        return GetAllLicenses(wikiMarkup).Except(GetApplicableLicenses(wikiMarkup)).ToList();
    }

    
    public static License GetMainLicense(string wikiMarkup)
    {
        var licenseList = GetAllLicenses(wikiMarkup).ToList();

        SortLicenses(licenseList);
        
        return licenseList.FirstOrDefault();
    }

    public static int GetMainLicenseId(string wikiMarkup)
    {
        return GetMainLicense(wikiMarkup) != null ? GetMainLicense(wikiMarkup).Id : -1;
    }

    public static List<License> SortLicenses(List<License> licenseList)
    {
        return licenseList
            .OrderBy(license => license.GetLicenseCategory().GetIntValue())
            .ThenByDescending(license => new GetLicenseComponents(license).CcVersion)
            .ThenBy(PriotizeByCcJurisdictionToken)
            .ThenBy(license => license.WikiSearchString)
            .ToList();
    }

    public static int PriotizeByCcJurisdictionToken(License license)
    {
        var licenseComponents = new GetLicenseComponents(license);

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
            .Except(GetAllLicenses(wikiMarkup).Select(license => license.WikiSearchString))
            .ToList();
    }
}





