using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;
using TrueOrFalse.WikiMarkup;

public class LicenseRepository
{
    public static List<License> GetAll()
    {
        return new List<License>()
        {
            new License(true) {Id = 1, Name = "test", SearchString = "cc-by-sa-3.0"},
            new License {Id = 991, Name = "CC", AuthorRequired = true},
            new License {Id = 992, Name = "KOREAN-STAMPS", AuthorRequired = true},
            //cc-by-sa-3.0,2.5,2.0,1.0
        };
    }
}


public class LicenseParser
{
    public static IList<License> Run(string wikiMarkup)
    {
        var tokenizedMarkup = ParseTemplate.TokenizeMarkup(wikiMarkup);

        return LicenseRepository.GetAll()
            .Where(license => tokenizedMarkup.Any(x => !String.IsNullOrEmpty(license.SearchString) && x.ToLower() == license.SearchString.ToLower())).ToList();

    }
}