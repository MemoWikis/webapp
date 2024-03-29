using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse
{
    public class WikiImageLicenseLoader : IRegisterAsInstancePerLifetime
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WikiImageLicenseLoader(IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public WikiImageLicenseInfo Run(string imageTitle, 
            string apiHost)
        {
            var markup = LoadMarkupdFromWikipedia(imageTitle, apiHost);
            return WikiImageLicenseInfo.ParseMarkup(markup);
        }

        private string LoadMarkupdFromWikipedia(string imageTitle, string apiHost)
        {
            imageTitle = WikiApiUtils.ExtractFileNameFromUrl(imageTitle);

            var url = $"http://{apiHost}/w/index.php?title=File:{imageTitle}&action=raw";

            string markup = "";

            try
            {
                markup = WikiApiUtils.GetWebpage(url);
            }
            catch (Exception e)
            {
                Logg.r.Error(e, "Could not load markup: {url}", url);
            }

            return markup;
        }
    }
}