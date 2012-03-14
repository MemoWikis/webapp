using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public class TimeElapsedAsText
    {
        /// <summary>
        /// 34 Sekunden
        /// 3 Minuten
        /// 2 Stunden
        /// 22 Tagen
        /// </summary>
        /// <returns></returns>
        static public string  Run(DateTime dateTime)
        {
            var elapsedTime = DateTime.Now - dateTime;

            if (elapsedTime.TotalSeconds <= 60)
                return elapsedTime.TotalSeconds + " Sekunden";

            if (elapsedTime.TotalMinutes <= 60)
                return elapsedTime.TotalMinutes + " Minuten";

            if (elapsedTime.TotalHours <= 24)
                if ((int)Math.Round(elapsedTime.TotalHours,0) == 1)
                    return "1 Stunde";
                else
                    return (int)elapsedTime.TotalHours + " Stunden";

            return "";
        }
    }
}
