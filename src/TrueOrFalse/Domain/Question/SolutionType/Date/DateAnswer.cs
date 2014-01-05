using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class DateAnswer
    {
        public bool IsValid;
        public string Input;
        public DatePrecision Precision;

        public int Day;
        public int Month;
        public int Year;

        public bool Valid(DateAnswer dateAnswer, DatePrecision precisionToCompare)
        {
            if (precisionToCompare == DatePrecision.Millenium)
                return this.Year == dateAnswer.Year;


            if (precisionToCompare == DatePrecision.Century)
                return this.Year == dateAnswer.Year;

            if (precisionToCompare == DatePrecision.Decade)
                return this.Year == dateAnswer.Year;

            if (precisionToCompare == DatePrecision.Year)
                return this.Year == dateAnswer.Year;

            if (precisionToCompare == DatePrecision.Month)
                return this.Year == dateAnswer.Year && 
                    this.Month == dateAnswer.Month;

            if (precisionToCompare == DatePrecision.Day)
                return this.Year == dateAnswer.Year &&
                    this.Month == dateAnswer.Month &&
                    this.Day== dateAnswer.Day;

            return false;
        }
    }
}
