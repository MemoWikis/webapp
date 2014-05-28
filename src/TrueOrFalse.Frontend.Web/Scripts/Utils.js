var Utils = (function () {
    function Utils() {
    }
    Utils.Random = function (minVal, maxVal, floatVal) {
        if (typeof floatVal === "undefined") { floatVal = 'undefined'; }
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return (typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    };
    return Utils;
})();
//# sourceMappingURL=Utils.js.map
