var ResponsiveBootstrapToolkit: any;

class Utils
{
    static UIMessageHtml(text: string, type: string): string {
        var cssClass = "info";

        if (type === "danger"
            || type === "warning"
            || type === "success") {
            cssClass = type;
        }

        return "<div class='alert alert-" + cssClass
            + " fade in'><a class='close' data-dismiss='alert' href='#'>×</a>" + text + "</div>";
    }

    static Random(minVal: any, maxVal: any, floatVal: any = 'undefined'): number
    {
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return <number>(typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    }

    static SetElementValue(selector: string, newValue: string) {
        Utils.SetElementValue2($(selector), newValue);
    }

    static SetElementValue2(elements: JQuery, newValue: string) {
        elements.text(newValue);
        Utils.Hightlight(elements);
    }

    static Hightlight(elements: JQuery) : JQuery {
        elements
            .animate({ opacity: 0.25 }, 100)
            .animate({ opacity: 1.00 }, 800);

        return elements;
    }

    static SetMenuPins(newAmount){
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount);
    }

    static MenuPinsPluseOne() {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html()); newAmount += 1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    }

    static MenuPinsMinusOne() {
        var newAmount = parseInt($("#menuWishKnowledgeCount").html()); newAmount += -1;
        Utils.SetElementValue("#menuWishKnowledgeCount", newAmount.toString());
    }


    static DisplayBreakpointOnResize() {
        $(window).resize(() => {
            Utils.DisplayBreakpoint();
        });
    }

    static DisplayBreakpoint() {
        if ($(window).width() < 768) {
            window.console.log("xs " + $(window).width());
        }
        else if ($(window).width() >= 768 && $(window).width() <= 992) {
            window.console.log("sm " + $(window).width());
        }
        else if ($(window).width() > 992 && $(window).width() <= 1200) {
            window.console.log("md " + $(window).width());
        }
        else {
            window.console.log("lg  " + $(window).width());
        }
    }
}

