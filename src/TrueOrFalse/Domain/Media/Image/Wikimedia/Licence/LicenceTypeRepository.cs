using System.Collections.Generic;
using TrueOrFalse;

public class LicenceTypeRepository
{
    public List<Licence> GetAll()
    {
        return new List<Licence>()
        {
            new Licence {Id = 1, Name = "CC", AuthorRequired = true},
            new Licence {Id = 1, Name = "KOREAN-STAMPS", AuthorRequired = true}
        };
    }
}

public class L

public class LicenceParser
{
    public IList<Licence> Run(string wikiMarkup)
    {
        
    }
}