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
        $(".removeRelevance").click(function() {
            var sliderContainer = $(this).parentsUntil(".column-1");
            sliderContainer.hide();
            sliderContainer.parent().find(".addRelevance").show();
            self.SendSilderValue(sliderContainer.find(".slider"), -1);

        });

        $(".addRelevance").click(function () {
            $(this).hide();
            $(this).parent().find(".sliderContainer").show();
            $(this).parent().parent().find(".sliderAnotation").show();
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
            slide: function(event, ui){ self.SetUiSliderSpan($(this), ui.value); },
            change: function(event, ui){ self.SendSilderValue($(this), ui.value); }
        });
    }

    SetUiSliderSpan(divSlider, sliderValueParam) {
        var text = sliderValueParam != "-1" ? (sliderValueParam / 10).toString() : "";
        divSlider.parent().find(".sliderValue").text(text);
    }

    SendSilderValue(divSlider, sliderValueParam) {
        $.ajax({
            type: 'POST',
            url:
                this._mode ==  ValuationPerRowMode.Question ? 
                    "/Questions/SaveRelevancePersonal/" + divSlider.attr("data-questionId") + "/" +sliderValueParam:
                    "/Sets/SaveRelevancePersonal/" + divSlider.attr("data-setId") + "/" + sliderValueParam,
            cache: false,
            success: function (result) {
                divSlider.parent().parent().find(".totalRelevanceEntries").text(result.totalValuations.toString());
                divSlider.parent().parent().find(".totalRelevanceAvg").text(result.totalAverage.toString());

                if (result.totalWishKnowledgeCountChange) {
                    this.SetMenuWishKnowledge(result.totalWishKnowledgeCount);
                    $("#tabWishKnowledgeCount").text(result.totalWishKnowledgeCount)
                    .animate({ opacity: 0.25 }, 100)
                    .animate({ opacity: 1.00 }, 500);
                }
            }
        });
    }

    SetMenuWishKnowledge(newAmount) {
        $("#menuWishKnowledgeCount")
            .text(newAmount)
            .animate({ opacity: 0.25 }, 100)
            .animate({ opacity: 1.00 }, 500);
    }
}