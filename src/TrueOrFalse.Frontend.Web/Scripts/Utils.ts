class Utils
{
    static Random(minVal: any, maxVal: any, floatVal: any = 'undefined'): number
    {
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return <number>(typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    }


    static SetElementValue(selector: string, newValue: string) {
        Utils.SetElementValue2($(selector), newValue);
    }

    static SetElementValue2(elements: JQuery, newValue: string) {
        elements
            .text(newValue)
            .animate({ opacity: 0.25 }, 100)
            .animate({ opacity: 1.00 }, 800);        
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
}

