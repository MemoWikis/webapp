/// <reference path="typescript.defs/qunit.d.ts" />
/// <reference path="SolutionMetaData.ts" />
var DateR = (function () {
    function DateR() {
        this.IsInvalid = true;
    }
    return DateR;
})();

var DateParser = (function () {
    function DateParser() {
    }
    DateParser.Run = function (input) {
        if (typeof input != 'string' && !(input instanceof String))
            return { Input: input };

        if (!this.ParseForDate(input).IsInvalid)
            return this._lastResult;
        if (!this.ParseForMonth(input).IsInvalid)
            return this._lastResult;
        if (!this.ParseForYear(input).IsInvalid)
            return this._lastResult;

        return { Input: input };
    };

    DateParser.ParseForDate = function (input) {
        var parts = input.split('.');

        if (parts.length != 3)
            return {};

        var day = parts[0];
        var month = parts[1];
        var year = parts[2];

        return this._lastResult = {};
    };

    DateParser.ParseForMonth = function (input) {
        return this._lastResult = {};
    };

    DateParser.ParseForYear = function (input) {
        return this._lastResult = {};
    };
    return DateParser;
})();

var DateParserTests = (function () {
    function DateParserTests() {
    }
    DateParserTests.Run = function () {
        test("title", function () {
            var date = DateParser.Run("");
            equal(date.IsInvalid, false);
        });
    };
    return DateParserTests;
})();
