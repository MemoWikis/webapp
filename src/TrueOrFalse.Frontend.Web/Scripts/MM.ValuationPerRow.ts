/// <reference path="typescript.defs/jquery.d.ts" />
/// <reference path="typescript.defs/bootstrap.d.ts" />

enum ValuationPerRowMode
{
    Question,
    Set
}

class ValuationPerRow
{
    private _mode: ValuationPerRowMode;

    constructor(parentDiv: string, mode: ValuationPerRowMode)
    {
        this._mode = mode;

        var self = this;
        $(".removeRelevance").click(function (e) {
            e.preventDefault();
            var sliderContainer = $(this).parentsUntil(parentDiv);
            sliderContainer.hide();
            sliderContainer.parent().find(".addRelevance").show();
            self.SendSilderValue(sliderContainer.find(".slider"), -1, self);

        });

        $(".addRelevance").click(function (e) {
            e.preventDefault();
            $(this).hide();
            $(this).parent().find(".sliderContainer").show();
            $(this).parent().parent().find(".sliderAnnotation").show();
            var slider = $(this).parent().find(".slider");
            self.SetUiSliderSpan(slider, 0);
            self.InitSlider(slider.parent().parent());
        });

        $(parentDiv).each(function () {
            self.InitSlider($(this));
        });        
    }
    
    InitSlider(divColumn1) {

        var self = this;
        var sliderValue = divColumn1.find(".sliderValue").text();
        divColumn1.find(".sliderValue").text(sliderValue / 10);
        divColumn1.find(".slider").slider({
            handle: "#myhandle",
            range: "min",
            max: 100,
            value: sliderValue,
            slide: function (event, ui) { self.SetUiSliderSpan($(this), ui.value); },
            change: function (event, ui) { self.SendSilderValue($(this), ui.value, self); }
        });
    }

    SetUiSliderSpan(divSlider, sliderValueParam) {
        var text = sliderValueParam != "-1" ? (sliderValueParam / 10).toString() : "";
        divSlider.parent().find(".sliderValue").text(text);
    }

    SendSilderValue(divSlider, sliderValueParam, self: ValuationPerRow) {
        var _this = this;
        $.ajax({
            type: 'POST',
            url:
                self._mode ==  ValuationPerRowMode.Question ? 
                    "/Questions/SaveRelevancePersonal/" + divSlider.attr("data-questionId") + "/" +sliderValueParam:
                    "/Sets/SaveRelevancePersonal/" + divSlider.attr("data-setId") + "/" + sliderValueParam,
            cache: false,
            success: function (result) {
                divSlider.parent().parent().find(".totalRelevanceEntries").text(result.totalValuations.toString());
                divSlider.parent().parent().find(".totalRelevanceAvg").text(result.totalAverage.toString());

                if (result.totalWishKnowledgeCountChange) {
                    if (self._mode == ValuationPerRowMode.Question) {
                        _this.SetMenuWishKnowledge(result.totalWishKnowledgeCount);
                    }
                    _this.SetElementValue(".tabWishKnowledgeCount", result.totalWishKnowledgeCount);
                }
            }
        });
    }

    SetMenuWishKnowledge(newAmount) {
        this.SetElementValue("#menuWishKnowledgeCount", newAmount);
    }

    SetElementValue(selector: string, newValue : string) {
        $(selector)
            .text(newValue)
            .animate({ opacity: 0.25 }, 100)
            .animate({ opacity: 1.00 }, 500);
    }
}