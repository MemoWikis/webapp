using System.Text.RegularExpressions;
using Seedworks.Lib;

public class DateAnswerParser
{
    public static DateAnswer Run(string input)
    {
        var result = new DateAnswer {Input = input};

        if (String.IsNullOrEmpty(input))
            return result;

        var parts = input.Split('.');

        if (input.IndexOf("Jh") > 0 || input.IndexOf("Jh.") > 0)
        {
            var userInput = input.Replace("Jh", "").Replace("Jh.", "").Trim();
            if (!userInput.IsNumeric())
                return result;

            result.IsValid = true;
            result.Year = userInput.ToInt();
            result.Precision = DatePrecision.Century;
            return result;
        }
            
        if (input.IndexOf("Jt") > 0 || input.IndexOf("Jt.") > 0)
        {
            var userInput = input.Replace("Jt", "").Replace("Jt.", "").Trim();

            if (!new Regex("^\\d{1,7}$").IsMatch(userInput))
                return result;

            result.IsValid = true;
            result.Year = userInput.ToInt();
            result.Precision = DatePrecision.Millenium;
            return result;
        }
            
        if (parts.Length == 3)
        { //DAY

            result.IsValid = true;
            result.Day = parts[0].ToInt();
            result.Month = parts[1].ToInt();
            result.Year = parts[2].ToInt();

            result.Precision = DatePrecision.Day;
            return result;
        }
            
        if (parts.Length == 2)
        { //Month 

            result.IsValid = true;
            result.Month = parts[0].ToInt();
            result.Year = parts[1].ToInt();

            result.Precision = DatePrecision.Month;
            return result;
        } 
            
        if (new Regex("^[-]{0,1}[ ]*\\d{1,10}").IsMatch(input))
        {//Year
            var userInput = input.Replace(" ", "").Replace("-", "").Trim();

            result.IsValid = true;
            result.Precision = DatePrecision.Year;
            result.Year = userInput.ToInt();
            return result;
        }

        return result;
    }
        
}