var DateR = (function () {
    function DateR() {
        this.IsInvalid = true;
    }
    return DateR;
})();
var DateParser = (function () {
    function DateParser() { }
    DateParser.Run = function Run(input) {
        if(typeof input != 'string' && !(input instanceof String)) {
            return {
                Input: input
            };
        }
        if(!this.ParseForDate(input).IsInvlid) {
            return DateParser._lastResult;
        }
        if(!this.ParseForMonth(input).IsInvlid) {
            return DateParser._lastResult;
        }
        if(!this.ParseForYear(input).IsInvlid) {
            return DateParser._lastResult;
        }
        return {
            Input: input
        };
    };
    DateParser.ParseForDate = function ParseForDate(input) {
        var parts = input.split('.');
        if(parts.length != 3) {
            return {
            };
        }
        var day = parts[0];
        var month = parts[1];
        var year = parts[2];
        return DateParser._lastResult = {
        };
    };
    DateParser.ParseForMonth = function ParseForMonth(input) {
        return DateParser._lastResult = {
        };
    };
    DateParser.ParseForYear = function ParseForYear(input) {
        return DateParser._lastResult = {
        };
    };
    return DateParser;
})();
