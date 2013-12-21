/// <reference path="typescript.defs/qunit.d.ts" />
/// <reference path="SolutionMetaData.ts" />


class DateR{
    IsInvalid: boolean = true;

    Input: string;
    Precision: DatePrecision;

    Day: number;
    Month: number;
    Year: number;

    IsNegative : boolean = false; 

    constructor(input : string) {
        this.Input = input;
    }

    ToLabel(): string {

        var monthNames = ["Januar", "Februar", "M&#228;rz",
            "April", "Mai", "Juni", "Juli", "August", "September",
            "Oktober", "November", "Dezember"];

        switch (this.Precision) {
            case DatePrecision.Day:
                var date = new Date(this.Year, this.Month - 1, this.Day);
                return date.getMonth() + ". " + monthNames[date.getMonth()] + " " + date.getFullYear();
            case DatePrecision.Month:
                var date = new Date(this.Year, this.Month - 1);
                return monthNames[date.getMonth()] + " " + date.getFullYear();
            case DatePrecision.Year:

                if (this.IsNegative && this.Year > 10000)
                    return "vor " + this.Year.toString() + " Jahren";

                if (this.IsNegative)
                    return this.Year.toString() + " v. Chr.";

                return this.Year.toString(); 
            case DatePrecision.Decade:
                return "Dekade"; 
            case DatePrecision.Century:
                return this.Year + ". Jahrhundert";
            case DatePrecision.Millenium:
                return this.Year + ". Jahrausend";
        }
        return "";
    }
}

class DateParser{

    private static _lastResult;

    static Run(input: any): DateR {

        if (typeof input != 'string' && !(input instanceof String))
            return new DateR(input);

        if (!this.Parse(input).IsInvalid) return this._lastResult;

        return new DateR(input);
    }

    private static Parse(input : string): DateR {

        var parts = input.split('.');

        var result = new DateR(input);

        if (input.indexOf("Jh") > 0 || input.indexOf("Jh.") > 0) {

            var userInput = input.replace("Jh", "").replace("Jh.", "");

            if (parseInt(userInput) == NaN)
                return result;

            userInput = userInput.trim();

            if (!/^\d{1,8}$/.test(userInput))
                return result;

            result.IsInvalid = false;
            result.Year = parseInt(userInput);
            result.Precision = DatePrecision.Century;
            return this._lastResult = result; 

        } else if (input.indexOf("Jt") > 0 || input.indexOf("Jt.") > 0) {
            
            var userInput = input.replace("Jt", "").replace("Jt.", "");

            if (parseInt(userInput) == NaN)
                return result;

            userInput = userInput.trim();

            if (!/^\d{1,7}$/.test(userInput))
                return result;

            result.IsInvalid = false;
            result.Year = parseInt(userInput);
            result.Precision = DatePrecision.Millenium;
            return this._lastResult = result; 

        } else if (parts.length == 3) { //DAY

            result.Day = parseInt(parts[0]);
            result.Month = parseInt(parts[1]);
            result.Year = parseInt(parts[2]);

            var date = new Date(result.Year, result.Month - 1, result.Day);
            if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month && date.getDate() == result.Day)
                result.IsInvalid = false;

            result.Precision = DatePrecision.Day;
            return this._lastResult = result;   

        } else if (parts.length == 2) { //Month 

            result.Month = parseInt(parts[0]);
            result.Year = parseInt(parts[1]);

            var date = new Date(result.Year, result.Month - 1);
            if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month)
                result.IsInvalid = false;

            result.Precision = DatePrecision.Month;
            return this._lastResult = result; 

        } else if (/^[-]{0,1}[ ]*\d{1,10}$/.test(input.trim())) { //Year

            var userInput = input.replace(/ /g, "");

            if (userInput.indexOf("-") == 0) {
                result.IsNegative = true;
                userInput = input.replace("-", "");
            }
            result.IsInvalid = false;
            result.Precision = DatePrecision.Year;
            result.Year = parseInt(userInput);
            return this._lastResult = result; 
        }


        return new DateR(input);
    }

    private static ParseForMonth(input: string): DateR {
        return this._lastResult = new DateR(input);
    }

    private static ParseForYear(input: string): DateR {
        return this._lastResult = new DateR(input);
    }
}

class DateParserTests {
    public static Run() {
        test("DateParser tests", function () {
            equal(DateParser.Run("abc").IsInvalid, true);

            equal(DateParser.Run("22.12.2014").IsInvalid, false);
            equal(DateParser.Run("22.12.2014").Day, 22);
            equal(DateParser.Run("22.12.2014").Month, 12);
            equal(DateParser.Run("22.12.2014").Year, 2014);
            equal(DateParser.Run("22.12.2014").Precision, DatePrecision.Day);

            equal(DateParser.Run("12.2014").IsInvalid, false);
            equal(DateParser.Run("12.2014").Month, 12);
            equal(DateParser.Run("12.2014").Year, 2014);
            equal(DateParser.Run("12.2014").Precision, DatePrecision.Month);

            equal(DateParser.Run("3 Jh").IsInvalid, false);
            equal(DateParser.Run("3 Jh").Year, 3);
            equal(DateParser.Run("3 Jh").Precision, DatePrecision.Century);
            equal(DateParser.Run("3 : Jh").IsInvalid, true);
            equal(DateParser.Run("31 10 Jh").IsInvalid, true);
            equal(DateParser.Run("Jh").IsInvalid, true);

            equal(DateParser.Run("5 Jt").IsInvalid, false);
            equal(DateParser.Run("5 Jt").Year, 5);
            equal(DateParser.Run("5 Jt").Precision, DatePrecision.Millenium);

            equal(DateParser.Run("1999").IsInvalid, false);
            equal(DateParser.Run("1999").Year, 1999);
            equal(DateParser.Run("1999").Precision, DatePrecision.Year);
        });
    }
}
