var ValuationPerRow = (function () {
    function ValuationPerRow() {
        var _this = this;
        $(".removeRelevance").click(function () {
            var sliderContainer = $(_this).parentsUntil(".column-1");
            sliderContainer.hide();
            sliderContainer.parent().find(".addRelevance").show();
            _this.SendSilderValue(sliderContainer.find(".slider"), -1);
        });
        $(".addRelevance").click(function () {
            $(_this).hide();
            $(_this).parent().find(".sliderContainer").show();
            $(_this).parent().parent().find(".sliderAnotation").show();
            var slider = $(_this).parent().find(".slider");
            _this.SetUiSliderSpan(slider, 0);
            _this.InitSlider(slider.parent().parent());
        });
        $(".column-1").each(function () {
            _this.InitSlider($(_this));
        });
    }
    ValuationPerRow.prototype.InitSlider = function (divColumn1) {
        var sliderValue = divColumn1.find(".sliderValue").text();
        divColumn1.find(".sliderValue").text(sliderValue / 10);
        console.log(sliderValue);
        divColumn1.find(".slider").slider({
            handle: "#myhandle",
            range: "min",
            max: 100,
            value: sliderValue
        });
    };
    ValuationPerRow.prototype.SetUiSliderSpan = function (divSlider, sliderValueParam) {
        var text = sliderValueParam != "-1" ? (sliderValueParam / 10).toString() : "";
        divSlider.parent().find(".sliderValue").text(text);
    };
    ValuationPerRow.prototype.SendSilderValue = function (divSlider, sliderValueParam) {
        $.ajax({
            type: 'POST',
            url: "/Questions/SaveRelevancePersonal/" + divSlider.attr("data-questionId") + "/" + sliderValueParam,
            cache: false,
            success: function (result) {
                divSlider.parent().parent().find(".totalRelevanceEntries").text(result.totalValuations.toString());
                divSlider.parent().parent().find(".totalRelevanceAvg").text(result.totalAverage.toString());
                if(result.totalWishKnowledgeCountChange) {
                    this.SetMenuWishKnowledge(result.totalWishKnowledgeCount);
                    $("#tabWishKnowledgeCount").text(result.totalWishKnowledgeCount).animate({
                        opacity: 0.25
                    }, 100).animate({
                        opacity: 1.00
                    }, 500);
                }
            }
        });
    };
    ValuationPerRow.prototype.SetMenuWishKnowledge = function (newAmount) {
        $("#menuWishKnowledgeCount").text(newAmount).animate({
            opacity: 0.25
        }, 100).animate({
            opacity: 1.00
        }, 500);
    };
    return ValuationPerRow;
})();
$(function () {
    new ValuationPerRow();
});
