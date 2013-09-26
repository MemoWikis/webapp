/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class ValuationPerRow
{
    constructor() {

        $(".removeRelevance").click(() => {
            var sliderContainer = $(this).parentsUntil(".column-1");
            sliderContainer.hide();
            sliderContainer.parent().find(".addRelevance").show();
            this.SendSilderValue(sliderContainer.find(".slider"), -1);

        });

        $(".addRelevance").click(() => {
            $(this).hide();
            $(this).parent().find(".sliderContainer").show();
            $(this).parent().parent().find(".sliderAnotation").show();
            var slider = $(this).parent().find(".slider");
            this.SetUiSliderSpan(slider, 0);
            this.InitSlider(slider.parent().parent());
        });

        $(".column-1").each(() => {
            this.InitSlider($(this));
        });
    }

    InitSlider(divColumn1) {
        var sliderValue = divColumn1.find(".sliderValue").text();
        divColumn1.find(".sliderValue").text(sliderValue / 10);
        console.log(sliderValue);
        divColumn1.find(".slider").slider({
            handle: "#myhandle",
            range: "min",
            max: 100,
            value: sliderValue,
            //slide: (event, ui) => { this.SetUiSliderSpan($(this), ui.value); },
            //change: (event, ui) => { this.SendSilderValue($(this), ui.value); }
        });
    }

    SetUiSliderSpan(divSlider, sliderValueParam) {
        var text = sliderValueParam != "-1" ? (sliderValueParam / 10).toString() : "";
        divSlider.parent().find(".sliderValue").text(text);
    }

    SendSilderValue(divSlider, sliderValueParam) {
        $.ajax({
            type: 'POST',
            url: "/Questions/SaveRelevancePersonal/" + divSlider.attr("data-questionId") + "/" + sliderValueParam,
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

$(function () {
    new ValuationPerRow();
});