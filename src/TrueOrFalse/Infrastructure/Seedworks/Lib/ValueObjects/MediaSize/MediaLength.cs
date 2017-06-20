using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.ValueObjects
{
	/// <summary>
	/// Why not use TimeSpan internally instead of seconds? Less conversion code. 
	/// </summary>
    public class MediaLength
    {
        public int Seconds;

        public MediaLength() { }

        /// <param name="seconds">seconds</param>
        public MediaLength(int seconds)
        {
            Seconds = seconds;
        }

        /// <param name="value">Expected Format: hh:mm:ss</param>
        public MediaLength(string value)
        {
            ParseAndSet(value);
        }

        /// <summary>
        /// Accepted File Formats: hh:mm:ss
        /// </summary>
        public void ParseAndSet(string value)
        {
            string[] parts = value.Split(':');

            Seconds = Seconds + 3600 * Convert.ToInt32(parts[0]);
            Seconds = Seconds + 60 * Convert.ToInt32(parts[1]);
            Seconds = Seconds + Convert.ToInt32(parts[2]);
        }

        public override string ToString()
        {
            int remainderHours = 0;
            int hours = Math.DivRem(Seconds, 3600, out remainderHours);
            int reminderMinutes = 0;

            int minutes = Math.DivRem(remainderHours, 60, out reminderMinutes);
            int seconds = reminderMinutes;

            return String.Format("{00:00}", hours) + ":" + String.Format("{00:00}", minutes) + ":" + String.Format("{00:00}", seconds);
        }
    }
}
