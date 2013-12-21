/// <reference path="typescript.defs/qunit.d.ts" />
/// <reference path="SolutionMetaData.ts" />


class DateR{
    IsInvalid: boolean = true;

    Input: string;
    Precision: DatePrecision;

    Day: number;
    Month: number;
    Year: number;

    constructor(input : string) {
        this.Input = input;
    }

    ToLabel(): string {

        var monthNames = ["Januar", "Februar", "März",
            "April", "Mai", "Juni", "Juli", "August", "September",
            "Oktober", "November", "Dezember"];

        switch (this.Precision) {
            case DatePrecision.Day:
                var date = new Date(this.Year, this.Month - 1, this.Day);
                return date.getMonth() + ". " + monthNames[date.getMonth()] + " " + date.getFullYear();
            case DatePrecision.Month:
                return "Monat"; 
            case DatePrecision.Year:
                return "Jahr"; 
            case DatePrecision.Decade:
                return "Dekade"; 
            case DatePrecision.Century:
                return "Century";
            case DatePrecision.Millenium:
                return "Millenium";
        }
        return "";
    }
}

class DateParser{

    private static _lastResult;

    static Run(input: any): DateR {

        if (typeof input != 'string' && !(input instanceof String))
            return new DateR(input);

        if (!this.ParseForDate(input).IsInvalid) return this._lastResult;
        if (!this.ParseForMonth(input).IsInvalid) return this._lastResult;
        if (!this.ParseForYear(input).IsInvalid) return this._lastResult;

        return new DateR(input);
    }

    private static ParseForDate(input : string): DateR {
        var parts = input.split('.');

        if (parts.length != 3)
            return new DateR(input);

        var result = new DateR(input);
        result.Day = parseInt(parts[0]);
        result.Month = parseInt(parts[1]);
        result.Year = parseInt(parts[2]);

        var date = new Date(result.Year, result.Month-1, result.Day);
        console.log(date);
        if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month && date.getDate() == result.Day)
            result.IsInvalid = false;

        result.Precision = DatePrecision.Day;

        return this._lastResult = result;
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
        });
    }
}
