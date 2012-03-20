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
        static public string Run(DateTime dateTime)
        {
            var elapsedTime = DateTime.Now - dateTime;

            if (elapsedTime.TotalSeconds <= 1)
                return "vor Augenblicken";

            if (elapsedTime.TotalSeconds <= 60)
                return elapsedTime.TotalSeconds + " Sekunden";

            if (elapsedTime.TotalMinutes <= 60)
                return elapsedTime.TotalMinutes + " Minuten";

            if (elapsedTime.TotalHours <= 24)
                if ((int)Math.Round(elapsedTime.TotalHours,0) == 1)
                    return "einer Stunde";
                else
                    return (int)elapsedTime.TotalHours + " Stunden";

            if (elapsedTime.TotalDays < 30)
                if ((int)Math.Round(elapsedTime.TotalDays, 0) == 1)
                    return "einem Tag";
                else
                    return (int) elapsedTime.TotalDays + " Tage";

            if(elapsedTime.TotalDays < 365)
                if ((int)Math.Round(elapsedTime.TotalDays / 30, 0) == 1)
                    return "einem Monat";
                else
                    return (int)Math.Round(elapsedTime.TotalDays / 30, 0) + " Monate";


            return "";
        }
    }
}
