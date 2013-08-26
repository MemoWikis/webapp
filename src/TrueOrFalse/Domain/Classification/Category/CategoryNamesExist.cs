using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class CategoryNamesExist : IRegisterAsInstancePerLifetime
{
    public string MissingCategory { get; private set; }

    public bool Yes(IEnumerable<string> categoryNames)
    {
        MissingCategory = categoryNames
            .FirstOrDefault(categoryName =>
                            ServiceLocator.Resolve<CategoryRepository>()
                                .GetByName(categoryName) == null);
        return MissingCategory == null;
    }

    public bool No(IEnumerable<string> categoryNames)
    {
        return !Yes(categoryNames);
    }
}