using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;

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

    public ErrorMessage GetErrorMsg(UrlHelper url)
    {
        return new ErrorMessage(
                string.Format("Die Kategorie <strong>'{0}'</strong> existiert nicht. " +
                              "Klicke <a href=\"{1}\">hier</a>, um Kategorien anzulegen.",
                              MissingCategory,
                              url.Action("Create", "EditCategory", new { name = MissingCategory })));
    }
}