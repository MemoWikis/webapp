/// <reference path="typescript.defs/qunit.d.ts" />
/// <reference path="SolutionMetaData.ts" />


class DateR{
    IsInvalid: boolean = true;

    Input: string;
    Precision: DatePrecision;

    Day: number;
    Month: number;
    Year: number;
}

class DateParser{

    private static _lastResult;

    static Run(input: any): DateR {

        if (typeof input != 'string' && !(input instanceof String))
            return <DateR> { Input: input };

        if (!this.ParseForDate(input).IsInvalid) return this._lastResult;
        if (!this.ParseForMonth(input).IsInvalid) return this._lastResult;
        if (!this.ParseForYear(input).IsInvalid) return this._lastResult;

        return <DateR> { Input: input };
    }

    private static ParseForDate(input : string): DateR {
        var parts = input.split('.');

        if (parts.length != 3)
            return <DateR> {};

        var day = parts[0];
        var month = parts[1];
        var year = parts[2];

        return this._lastResult = <DateR> {};
    }

    private static ParseForMonth(input: string): DateR {
        return this._lastResult = <DateR> {};
    }

    private static ParseForYear(input: string): DateR {
        return this._lastResult = <DateR> {};
    }
}

class DateParserTests {
    public static Run() {
        test("title", function () {

            var date = <DateR>DateParser.Run("");
            equal(date.IsInvalid, false);
        });
    }
}
