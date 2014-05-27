/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
$(function () {
    InitFeedbackSliders();

    function foo(d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id))
            return;
        js = d.createElement(s);
        js.id = id;
        js.src = "//connect.facebook.net/de_DE/all.js#xfbml=1&appId=128827270569993";
        fjs.parentNode.insertBefore(js, fjs);
    }
    foo(document, 'script', 'facebook-jssdk');

    //$("#iAdd").hover(
    //    function () { $(this).removeClass("fa-heart-o").addClass("fa-heart"); },
    //    function () { $(this).removeClass("fa-heart").addClass("fa-heart-o"); }
    //);
    $("#iAdd").click(function (e) {
        if ($(this).hasClass("fa-heart-o"))
            $(this).switchClass("fa-heart-o", "fa-heart");
        else
            $(this).switchClass("fa-heart", "fa-heart-o");

        e.preventDefault();
    });
});

function InitFeedbackSliders() {
    //Quality
    InitFeedbackSlider("Quality");
    InitFeedbackSlider("RelevancePersonal");
    InitFeedbackSlider("RelevanceForAll");
}

function InitFeedbackSlider(sliderName) {
    $("#remove" + sliderName + "Value").click(function () {
        $("#div" + sliderName + "Slider").hide();
        $("#div" + sliderName + "Add").show();
        SendSilderValue(sliderName, -1);
    });

    $("#select" + sliderName + "Value").click(function () {
        $("#div" + sliderName + "Slider").show();
        $("#div" + sliderName + "Add").hide();
    });

    var sliderValue = $("#slider" + sliderName + "Value").text();
    SetUiSliderSpan(sliderName, sliderValue);

    $("#slider" + sliderName).slider({
        range: "min",
        max: 100,
        value: sliderValue,
        slide: function (event, ui) {
            SetUiSliderSpan(sliderName, ui.value);
        },
        change: function (event, ui) {
            SendSilderValue(sliderName, ui.value);
        }
    });
}

function SetUiSliderSpan(sliderName, uiValue) {
    var text = uiValue != -1 ? (uiValue / 10).toString() : "";
    $("#slider" + sliderName + "Value").text(text);
}

function SendSilderValue(sliderName, value) {
    $.ajax({
        type: 'POST',
        url: "/Questions/Save" + sliderName + "/" + window.questionId + "/" + value,
        cache: false,
        success: function (result) {
            $("#span" + sliderName + "Count").text(result.totalValuations.toString());
            $("#span" + sliderName + "Average").text(result.totalAverage.toString());
        }
    });
}
//# sourceMappingURL=Page.js.map
