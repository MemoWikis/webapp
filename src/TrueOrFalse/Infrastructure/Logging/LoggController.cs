using System.Web.Mvc;

namespace TrueOrFalse.Infrastructure.Logging
{
    class LoggController
    {
        [HttpPost]
        public void SetLoggReport(string report)
        {
            Logg.r().Error(report);
        }
    }
}
