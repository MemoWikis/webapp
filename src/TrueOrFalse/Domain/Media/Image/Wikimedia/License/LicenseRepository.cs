using System.Collections.Generic;
using TrueOrFalse;

public class LicenseRepository
{
    public List<License> GetAll()
    {
        return new List<License>()
        {
            new License() {Id = 1, Name = "CC", AuthorRequired = true},
            new License {Id = 1, Name = "KOREAN-STAMPS", AuthorRequired = true}
        };
    }
}


public class LicenseParser
{
    public IList<License> Run(string wikiMarkup)
    {
        //temp
        return new List<License>();
    }
}