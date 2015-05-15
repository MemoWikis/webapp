var Utils = (function () {
    function Utils() {
    }
    Utils.UIMessageHtml = function (text, type) {
        var cssClass = "info";

        if (type === "danger" || type === "warning" || type === "success") {
            cssClass = type;
        }

        return "<div class='alert alert-" + cssClass + " fade in'><a class='close' data-dismiss='alert' href='#'>×</a>" + text + "</div>";
    };

    Utils.Random = function (minVal, maxVal, floatVal) {
        if (typeof floatVal === "undefined") { floatVal = 'undefined'; }
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return (typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    };

    Utils.SetElementValue = function (selector, newValue) {
        Utils.SetElementValue2($(selector), newValue);
    };

    Utils.SetElementValue2 = function (elements, newValue) {
        elements.text(newValue);
        Utils.Hightlight(elements);
    };

    Utils.Hightlight = function (elements) {
        elements.animate({ opacity: 0.25 }, 100).animate({ opacity: 1.00 }, 800);

        return elements;
    };

    Utils.SetMenuPins = function (newAmount) {
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount);
    };

    Utils.MenuPinsPluseOne = function () {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html());
        newAmount += 1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    };

    Utils.MenuPinsMinusOne = function () {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html());
        newAmount += -1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    };
    return Utils;
})();
//# sourceMappingURL=Utils.js.map
