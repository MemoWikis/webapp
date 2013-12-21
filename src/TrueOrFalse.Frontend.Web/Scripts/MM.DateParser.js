/// <reference path="typescript.defs/qunit.d.ts" />
/// <reference path="SolutionMetaData.ts" />
var DateR = (function () {
    function DateR(input) {
        this.IsInvalid = true;
        this.Input = input;
    }
    DateR.prototype.ToLabel = function () {
        var monthNames = [
            "Januar", "Februar", "M&#228;rz",
            "April", "Mai", "Juni", "Juli", "August", "September",
            "Oktober", "November", "Dezember"];

        switch (this.Precision) {
            case 1 /* Day */:
                var date = new Date(this.Year, this.Month - 1, this.Day);
                return date.getMonth() + ". " + monthNames[date.getMonth()] + " " + date.getFullYear();
            case 2 /* Month */:
                var date = new Date(this.Year, this.Month - 1);
                return monthNames[date.getMonth()] + " " + date.getFullYear();
            case 3 /* Year */:
                return "Jahr";
            case 4 /* Decade */:
                return "Dekade";
            case 5 /* Century */:
                return "Century";
            case 6 /* Millenium */:
                return "Millenium";
        }
        return "";
    };
    return DateR;
})();

var DateParser = (function () {
    function DateParser() {
    }
    DateParser.Run = function (input) {
        if (typeof input != 'string' && !(input instanceof String))
            return new DateR(input);

        if (!this.Parse(input).IsInvalid)
            return this._lastResult;

        return new DateR(input);
    };

    DateParser.Parse = function (input) {
        var parts = input.split('.');

        var result = new DateR(input);
        if (parts.length == 3) {
            result.Day = parseInt(parts[0]);
            result.Month = parseInt(parts[1]);
            result.Year = parseInt(parts[2]);

            var date = new Date(result.Year, result.Month - 1, result.Day);
            if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month && date.getDate() == result.Day)
                result.IsInvalid = false;

            result.Precision = 1 /* Day */;
            return this._lastResult = result;
        } else if (parts.length == 2) {
            result.Month = parseInt(parts[0]);
            result.Year = parseInt(parts[1]);

            var date = new Date(result.Year, result.Month - 1);
            if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month)
                result.IsInvalid = false;

            result.Precision = 2 /* Month */;
            return this._lastResult = result;
        }

        return new DateR(input);
    };

    DateParser.ParseForMonth = function (input) {
        return this._lastResult = new DateR(input);
    };

    DateParser.ParseForYear = function (input) {
        return this._lastResult = new DateR(input);
    };
    return DateParser;
})();

var DateParserTests = (function () {
    function DateParserTests() {
    }
    DateParserTests.Run = function () {
        test("DateParser tests", function () {
            equal(DateParser.Run("abc").IsInvalid, true);

            equal(DateParser.Run("22.12.2014").IsInvalid, false);
            equal(DateParser.Run("22.12.2014").Day, 22);
            equal(DateParser.Run("22.12.2014").Month, 12);
            equal(DateParser.Run("22.12.2014").Year, 2014);
            equal(DateParser.Run("22.12.2014").Precision, 1 /* Day */);

            equal(DateParser.Run("12.2014").IsInvalid, false);
            equal(DateParser.Run("12.2014").Month, 12);
            equal(DateParser.Run("12.2014").Year, 2014);
            equal(DateParser.Run("12.2014").Precision, 2 /* Month */);
        });
    };
    return DateParserTests;
})();
//# sourceMappingURL=MM.DateParser.js.map
