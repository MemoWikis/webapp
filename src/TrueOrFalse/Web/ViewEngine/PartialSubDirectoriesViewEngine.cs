using System.Linq;
using System.Web.Mvc;

namespace TrueOrFalse.Frontend.Web
{
    public class PartialSubDirectoriesViewEngine : WebFormViewEngine
    {
        public PartialSubDirectoriesViewEngine()
        {
            var partialAdditionalLocations = new[]{
                "~/Views/{1}/Partials/{0}.ascx",
                "~/Views/Shared/NotLoggedIn/{0}.ascx"
            };

            PartialViewLocationFormats = PartialViewLocationFormats.Union(partialAdditionalLocations).ToArray();
        }
    }
}