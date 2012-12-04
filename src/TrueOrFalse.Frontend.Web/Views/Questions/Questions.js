/************************/
/******* FILTER *********/
$(function () {
    $("#addFilterUserId").val("");
    $("#delFilterUserId").val("");
});

$(function () {
    $('.btn-filterByMe').click(function () {
        $(this).toggleClass('active');
        $('#FilterByMe').val($(this).hasClass('active'));
    });
    if ($('#FilterByMe').val().toLowerCase() == "true") {
        $('.btn-filterByMe').addClass('active');
    }

    $('.btn-filterByAll').click(function () {
        $(this).toggleClass('active');
        $('#FilterByAll').val($(this).hasClass('active'));
    });
    if ($('#FilterByAll').val().toLowerCase() == "true") {
        $('.btn-filterByAll').addClass('active');
    }

    $('.userPopover').popover({
        content: getPopupOverContent
    });
    function getPopupOverContent() {
        return "123, <b>456</b>";  
    }

    $('#btnSearch').click(function () {
        window.location = "/questions/search/" + $('#txtSearch').val();
    });
});



/************************/
/*** DELETE QUESTION ****/
var questionIdToDelete;
$(function () {
    $('a[href*=#modalDelete]').click(function () {
        questionIdToDelete = $(this).attr("data-questionId");
        populateDeleteQuestionId(questionIdToDelete);
    });

    $('#btnCloseQuestionDelete').click(function () {
        $("#modalDelete").modal('hide');
    });

    $('#confirmQuestionDelete').click(function () {
        deleteQuestion(questionIdToDelete);
        $("#modalDelete").modal('hide');
    });
});

function populateDeleteQuestionId(questionId) {
    $.ajax({
        type: 'POST',
        url: "/Questions/DeleteDetails/" + questionId,
        cache: false,
        success: function (result) {
            $("#spanQuestionTitle").html(result.questionTitle.toString());
        },
        error: function () {
             alert("Ein Fehler ist aufgetreten");
        }
    });
}

function deleteQuestion(questionId) {
        $.ajax({
        type: 'POST',
        url: "/Questions/Delete/" + questionId,
        cache: false,
        success:function (){window.location.reload();},
        error: function (result) { alert("Ein Fehler ist aufgetreten"); }
    });
}

/************************/
/******* SLIDER *********/
$(function () {

    $(".removeRelevance").click(function () {
        var sliderContainer = $(this).parentsUntil(".column-1");
        sliderContainer.hide();
        sliderContainer.parent().find(".addRelevance").show();
        SendSilderValue(sliderContainer.find(".slider"), -1);

    });

    $(".addRelevance").click(function () {
        $(this).hide();
        $(this).parent().find(".sliderContainer").show();
        $(this).parent().parent().find(".sliderAnotation").show();
        var slider = $(this).parent().find(".slider");
        SetUiSliderSpan(slider, 0);
        InitSlider(slider.parent().parent());
    });

    $(".column-1").each(function () {
        InitSlider($(this));
    });

    function InitSlider(divColumn1) {
        var sliderValue = divColumn1.find(".sliderValue").text();
        divColumn1.find(".sliderValue").text(sliderValue / 10);

        divColumn1.find(".slider").slider({
            range: "min",
            max: 100,
            value: sliderValue,
            slide: function (event, ui) { SetUiSliderSpan($(this), ui.value); },
            change: function (event, ui) { SendSilderValue($(this), ui.value); }
        });
    }

    function SetUiSliderSpan(divSlider, sliderValueParam) {
        var text = sliderValueParam != -1 ? sliderValueParam / 10 : "";
        divSlider.parent().find(".sliderValue").text(text);
    }

    function SendSilderValue(divSlider, sliderValueParam) {
        $.ajax({
            type: 'POST',
            url: "/Questions/SaveRelevancePersonal/" + divSlider.attr("data-questionId") + "/" + sliderValueParam,
            cache: false,
            success: function (result) {
                divSlider.parent().parent().find(".totalRelevanceEntries").text(result.totalValuations.toString());
                divSlider.parent().parent().find(".totalRelevanceAvg").text(result.totalAverage.toString());

                if (result.totalWishKnowledgeCountChange) {
                    SetMenuWishKnowledge(result.totalWishKnowledgeCount);
                    $("#tabWishKnowledgeCount").text(result.totalWishKnowledgeCount)
                    .animate({ opacity: 0.25 }, 100)
                    .animate({ opacity: 1.00 }, 500);                    
                }
            }
        });
    }
});

/************************/
/******* Charts *********/

$(function () {
    $(".pieTotals").each(function () {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#1BE022', 'red']
        });
    });

    $(".piePersonalRelevanceTotal").each(function () {
        
        var value = parseFloat($(this).attr("data-avg"));
        var height = (value * parseFloat(0.9)) + 5;

        $(this).sparkline([Math.round(value)], {
            type: 'pie',
            sliceColors: ['#ABC0FF'],
            height: Math.round(height),
            disableTooltips: 'true'
        });
    });

    $(".tristateHistory").sparkline([1, 1, 0, 1, -1, -1, 1, -1, 0], {
        type: 'tristate',
        barWidth: 2,
        posBarColor: '#05c105',
        negBarColor: '#ce1212',
        tooltipValueLookups: { map: { '-1': 'Falsch', '0': 'Versuch und Antwort gezeigt', '1': 'Richtig'} }
    });
    
    $('.viewsHistory').sparkline('html', { fillColor: false });
    $('.viewsHistory').sparkline([4, 1, 5, 7, 9, 9, 8, 7, 6, 6, 4, 7, 8, 4, 3, 2, 2, 5, 6, 7],
        { composite: true, fillColor: false, lineColor: 'red' });
});

/************************/
$(function () {
    $('#tabInfoMyKnowledge').click(function () {
        $("#modalTabInfoMyKnowledge").modal('show');
    });
});