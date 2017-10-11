
$(function () {


    $("#revertAction").click(function () {
        $("#modalRevertAction").modal();
    });

    hiding();
    disableSubmitButtonSouldBePressedEnter();
    $("#questionId").typeWatch(options);


    //Check in and Checkout
    $("#questions").on("click",
        ".row",
        function (e) {
            var checkbox = $(this).find("input");
            toggleCheckbox(e, checkbox);
        });
    // Button Click 
    $("#learnSetSave").on("click",
        function (e) {

            e.preventDefault();
            var selectedQuestionIds = [];


            $("#questions input:checked").each(function () {
                selectedQuestionIds.push($(this).attr("value"));
            });


            $.ajax({
                type: 'POST',
                url: "/EditSet/AddQuestionsToSet",
                data: { setid: EditSet.GetSetId(), questionIds: selectedQuestionIds },
                success: function (data) {
                    if (data.Status === true) {

                        $("#alertOutput").text("Fragen erfolgreich hinzugefügt und Lernset gespeichert.");
                        //1: load row html
                        $.post("/EditSet/GetHtmlRows",
                            { setid: EditSet.GetSetId(), questionIds: selectedQuestionIds },
                            function (data) {
                                for (var i = 0; i < data.length; i++) {

                                    $("#ulQuestions").append($(data[i])).fadeIn(400);
                                }

                            });

                        $("#revertAction").click(function () {
                            $("#modalRevertAction").modal();
                        });

                    } else {
                        $("#safeQuestions").removeClass("alert-success").addClass("alert-warning");
                        $("#alertOutput").text("Speichern fehlgeschlagen");
                    }
                },
                dataType: 'json',
                async: false
            });



            $("#safeQuestions").fadeIn();
            $("#questions").empty();
            $('#learnSetSave').hide();
            $("#questionId").val("");
            $("#resultHeading").hide();
            $(".alert-info").hide();

        });
});

var options = {
    callback: function () {
        $("#questions").empty();
        $("#safeQuestions").hide();
        $.post("/EditSet/Search",
            { setId: EditSet.GetSetId(), term: $("#questionId").val() },
            function(data) {

                if (data.Questions.length > 0) {
                    $("#resultHeading").fadeIn();
                    $('#questions').fadeIn();
                    for (var i = 0; i < data.Questions.length; i++) {
                        $("#questions")
                            .append($("<div class='questionRows row'>")
                                .append($("<div class='col-xs-1'>").append()
                                    .append($("<input type='checkbox'" + "value=" + data.Questions[i].Id + " />")))
                                .append($("<div class='col-xs-2'>")
                                    .append($("<image src=" +
                                        data.Questions[i].ImageUrl +
                                        " class='previewImage' >")))
                                .append($("<div class='col-xs-9'>")
                                    .append(data.Questions[i].question)
                                    .append("<div class='greyed' style='font-size: smaller;'>Richtige Antwort: " + data.Questions[i].correctAnswer + "</div>")
                                    .append($("<p class='linkQuestion'>")
                                        .append($("<a target='_blank' href=" +
                                            data.Questions[i].QuestionUrl +
                                            ">zur Frage </a>"))))
                            );
                    }
                }

            });
    },
    wait: 750,
    highlight: true,
    allowSubmit: false,
    captureLength: 2,
    allowSameSearch: true
}


function toggleCheckbox(event, checkbox) {

    if (event.target.tagName.toUpperCase() !== 'INPUT') {
        if (checkbox.is(':checked')) {
            checkbox.prop('checked', false);
        } else {
            checkbox.prop('checked', true);
        }
    }

    if (checkbox.is(':checked')) {
        $('#learnSetSave').fadeIn();
    } else {

        var checkedCount = $("#questions input:checkbox")
            .filter(function (index, input) { return $(input).is(":checked") })
            .length;

        if (checkedCount === 0)
            $('#learnSetSave').hide();
    }

}

function disableSubmitButtonSouldBePressedEnter() {
    $('body').keypress(function (e) {
        if (e.keyCode === 13 && $("#questionId").is(":focus")) {
            event.preventDefault();
            return false;
        }
    });
}

function hiding() {
    $("#safeQuestions").hide();
    $("#resultHeading").hide();
    $('#questions').hide();
    $('#learnSetSave').hide();
}

