using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrueOrFalse.Infrastructure.Logging
{
    class LoggController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoggController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public void SetLoggReport(string report)
        {
            Logg.r.Error(report);
        }
    }
}
