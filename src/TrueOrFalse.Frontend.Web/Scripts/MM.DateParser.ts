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
        result.Precision = DatePrecision.Day;
        result.IsInvalid = false;

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
