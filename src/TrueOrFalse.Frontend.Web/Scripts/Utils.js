var ResponsiveBootstrapToolkit;

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

    Utils.DisplayBreakpointOnResize = function () {
        $(window).resize(function () {
            Utils.DisplayBreakpoint();
        });
    };

    Utils.DisplayBreakpoint = function () {
        if ($(window).width() < 768) {
            window.console.log("xs " + $(window).width());
        } else if ($(window).width() >= 768 && $(window).width() <= 992) {
            window.console.log("sm " + $(window).width());
        } else if ($(window).width() > 992 && $(window).width() <= 1200) {
            window.console.log("md " + $(window).width());
        } else {
            window.console.log("lg  " + $(window).width());
        }
    };
    return Utils;
})();
//# sourceMappingURL=Utils.js.map
