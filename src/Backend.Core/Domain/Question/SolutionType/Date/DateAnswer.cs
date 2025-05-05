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
        if (precisionToCompare == DatePrecision.Day || dateAnswer.Precision == DatePrecision.Day)
            return this.Year == dateAnswer.Year &&
                this.Month == dateAnswer.Month &&
                this.Day== dateAnswer.Day;

        if (precisionToCompare == DatePrecision.Month || dateAnswer.Precision == DatePrecision.Month)
            return this.Year == dateAnswer.Year &&
                this.Month == dateAnswer.Month;

        if (precisionToCompare == DatePrecision.Year || dateAnswer.Precision == DatePrecision.Year)
            return this.Year == dateAnswer.Year;

        if (precisionToCompare == DatePrecision.Decade || dateAnswer.Precision == DatePrecision.Decade)
            return this.Year == dateAnswer.Year;

        if (precisionToCompare == DatePrecision.Century || dateAnswer.Precision == DatePrecision.Century)
            return this.Year == dateAnswer.Year;

        if (precisionToCompare == DatePrecision.Millenium || dateAnswer.Precision == DatePrecision.Millenium)
            return this.Year == dateAnswer.Year;

        return false;
    }
}