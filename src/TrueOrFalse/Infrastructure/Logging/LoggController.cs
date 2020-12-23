using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
