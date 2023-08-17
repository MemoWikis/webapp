using Microsoft.AspNetCore.Mvc;

namespace TrueOrFalse.Infrastructure.Logging
{
    class LoggController
    {
        [HttpPost]
        public void SetLoggReport(string report)
        {
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(report);
        }
    }
}
