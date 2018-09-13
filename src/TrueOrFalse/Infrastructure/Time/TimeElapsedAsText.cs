using System;

namespace TrueOrFalse
{
    public class TimeElapsedAsText
    {

        public static string Run(DateTime dateTime)
        {
            return Run(dateTime, DateTime.Now);
        }

        /// <summary>
        /// 34 Sekunden
        /// 3 Minuten
        /// 2 Stunden
        /// 22 Tagen
        /// </summary>
        /// <returns></returns>
        public static string Run(DateTime dateTimeBegin, DateTime dateTimeEnd)
        {
            var elapsedTime = dateTimeEnd - dateTimeBegin;
            var calDaysPassed = dateTimeEnd.Date - dateTimeBegin.Date;

            if (elapsedTime.TotalSeconds < 60)
                return "weniger als einer Minute";

            if (elapsedTime.TotalSeconds < 90)
                return "einer Minute";

            if (elapsedTime.TotalMinutes < 60)
                return Math.Round(elapsedTime.TotalMinutes, MidpointRounding.AwayFromZero) + " Minuten";

            if (elapsedTime.TotalHours <= 24)
                if ((int)Math.Round(elapsedTime.TotalHours, MidpointRounding.AwayFromZero) == 1)
                    return "einer Stunde";
                else
                    return ((int)Math.Round(elapsedTime.TotalHours, MidpointRounding.AwayFromZero)) + " Stunden";

            if (elapsedTime.TotalDays < 30)
                if (calDaysPassed.TotalDays <= 1)
                    return "einem Tag";
                else
                    return (int) calDaysPassed.TotalDays + " Tagen";

            if(elapsedTime.TotalDays < 365)
                if ((int)Math.Round(elapsedTime.TotalDays / 30, 0) == 1)
                    return "einem Monat";
                else
                    return (int)Math.Round(elapsedTime.TotalDays / 30, 0) + " Monaten";


            if (elapsedTime.TotalDays < 365*1.5)
                return "einem Jahr";

            return Math.Round(elapsedTime.TotalDays/365) + " Jahren";

        }
    }
}
