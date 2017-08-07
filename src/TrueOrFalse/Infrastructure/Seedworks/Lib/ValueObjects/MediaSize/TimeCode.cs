using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.ValueObjects
{
    /// <summary>
    /// TimeCodes are used to identify a specific frame in a movie
    /// </summary>
    public class TimeCode
    {
        private int _milliSeconds;
       
        public int MilliSeconds 
        { 
            get{ return _milliSeconds; } 
            set{ _milliSeconds = value; } 
        }

        public int Seconds
        {
            get { return _milliSeconds / 1000; }
        }

        public string StringValue
        {
            get { return ToString(); }
        }

        public TimeCode(){}

        public TimeCode(string value)
        {
           ParseAndSet(value);
        }
        public TimeCode(int value)
        {
            _milliSeconds = value;
        }
        public void ParseAndSet(string value)
        {
            if (value.Contains(":"))
            {
                string[] parts = value.Split(':');

                //alles um 10 erhöht im vergleich zu medialenght, da anscheinend MS verwendet werden?!
                _milliSeconds = _milliSeconds + 3600000*Convert.ToInt32(parts[0]);
                _milliSeconds = _milliSeconds + 60000*Convert.ToInt32(parts[1]);
                _milliSeconds = _milliSeconds + 100*Convert.ToInt32(parts[2]);
            }
            else
                _milliSeconds = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            int reminderHours = 0;
            int hours = Math.DivRem(this._milliSeconds, 3600000, out reminderHours);
            int reminderMinutes = 0;
            int minutes = Math.DivRem(reminderHours, 60000, out reminderMinutes);
            int seconds = reminderMinutes/100;
            seconds = Convert.ToInt32(Math.Round(((decimal)seconds/10), 2));

            return String.Format("{00:00}", hours) + ":" + String.Format("{00:00}", minutes) + ":" + String.Format("{00:00}", seconds);
        }
    }
 }