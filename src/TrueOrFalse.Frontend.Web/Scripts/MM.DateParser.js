/// <reference path="typescript.defs/qunit.d.ts" />
/// <reference path="SolutionMetaData.ts" />
var DateR = (function () {
    function DateR(input) {
        this.IsValid = false;
        this.IsNegative = false;
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
                if (this.IsNegative && this.Year > 10000)
                    return "vor " + this.Year.toString() + " Jahren";

                if (this.IsNegative)
                    return this.Year.toString() + " v. Chr.";

                return this.Year.toString();
            case 4 /* Decade */:
                return "Dekade";
            case 5 /* Century */:
                return this.Year + ". Jahrhundert";
            case 6 /* Millenium */:
                return this.Year + ". Jahrausend";
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

        if (this.Parse(input).IsValid)
            return this._lastResult;

        return new DateR(input);
    };

    DateParser.Parse = function (input) {
        var parts = input.split('.');

        var result = new DateR(input);

        if (input.indexOf("Jh") > 0 || input.indexOf("Jh.") > 0) {
            var userInput = input.replace("Jh", "").replace("Jh.", "");

            if (parseInt(userInput) == NaN)
                return result;

            userInput = userInput.trim();

            if (!/^\d{1,8}$/.test(userInput))
                return result;

            result.IsValid = true;
            result.Year = parseInt(userInput);
            result.Precision = 5 /* Century */;
            return this._lastResult = result;
        } else if (input.indexOf("Jt") > 0 || input.indexOf("Jt.") > 0) {
            var userInput = input.replace("Jt", "").replace("Jt.", "");

            if (parseInt(userInput) == NaN)
                return result;

            userInput = userInput.trim();

            if (!/^\d{1,7}$/.test(userInput))
                return result;

            result.IsValid = true;
            result.Year = parseInt(userInput);
            result.Precision = 6 /* Millenium */;
            return this._lastResult = result;
        } else if (parts.length == 3) {
            result.Day = parseInt(parts[0]);
            result.Month = parseInt(parts[1]);
            result.Year = parseInt(parts[2]);

            var date = new Date(result.Year, result.Month - 1, result.Day);
            if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month && date.getDate() == result.Day)
                result.IsValid = true;

            result.Precision = 1 /* Day */;
            return this._lastResult = result;
        } else if (parts.length == 2) {
            result.Month = parseInt(parts[0]);
            result.Year = parseInt(parts[1]);

            var date = new Date(result.Year, result.Month - 1);
            if (date.getFullYear() == result.Year && date.getMonth() + 1 == result.Month)
                result.IsValid = true;

            result.Precision = 2 /* Month */;
            return this._lastResult = result;
        } else if (/^[-]{0,1}[ ]*\d{1,10}$/.test(input.trim())) {
            var userInput = input.replace(/ /g, "");

            if (userInput.indexOf("-") == 0) {
                result.IsNegative = true;
                userInput = input.replace("-", "");
            }
            result.IsValid = true;
            result.Precision = 3 /* Year */;
            result.Year = parseInt(userInput);
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
            equal(DateParser.Run("abc").IsValid, false);

            equal(DateParser.Run("22.12.2014").IsValid, true);
            equal(DateParser.Run("22.12.2014").Day, 22);
            equal(DateParser.Run("22.12.2014").Month, 12);
            equal(DateParser.Run("22.12.2014").Year, 2014);
            equal(DateParser.Run("22.12.2014").Precision, 1 /* Day */);

            equal(DateParser.Run("12.2014").IsValid, true);
            equal(DateParser.Run("12.2014").Month, 12);
            equal(DateParser.Run("12.2014").Year, 2014);
            equal(DateParser.Run("12.2014").Precision, 2 /* Month */);

            equal(DateParser.Run("3 Jh").IsValid, true);
            equal(DateParser.Run("3 Jh").Year, 3);
            equal(DateParser.Run("3 Jh").Precision, 5 /* Century */);
            equal(DateParser.Run("3 : Jh").IsValid, false);
            equal(DateParser.Run("31 10 Jh").IsValid, false);
            equal(DateParser.Run("Jh").IsValid, false);

            equal(DateParser.Run("5 Jt").IsValid, true);
            equal(DateParser.Run("5 Jt").Year, 5);
            equal(DateParser.Run("5 Jt").Precision, 6 /* Millenium */);

            equal(DateParser.Run("1999").IsValid, true);
            equal(DateParser.Run("1999").Year, 1999);
            equal(DateParser.Run("1999").Precision, 3 /* Year */);
        });
    };
    return DateParserTests;
})();
//# sourceMappingURL=MM.DateParser.js.map
