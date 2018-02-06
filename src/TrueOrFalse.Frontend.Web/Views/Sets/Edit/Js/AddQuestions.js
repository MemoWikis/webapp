
$(function () {


    $("#revertAction").click(function () {
        $("#modalRevertAction").modal();
    });

    hiding();
    disableSubmitButtonSouldBePressedEnter();
    $("#addQuestionSearchField").typeWatch(options);


    //Check in and Checkout
    $("#resultQuestions").on("click",
        ".row",
        function (e) {
            var checkbox = $(this).find("input");
            toggleCheckbox(e, checkbox);
        });

    // Button Click 
    $("#addMarkedQuestionsToSetAndSave").on("click",
        function (e) {

            $("#addMarkedQuestionsToSetAndSave").prop("disabled", true);
            e.preventDefault();
            var selectedQuestionIds = [];


            $("#resultQuestions input:checked").each(function () {
                selectedQuestionIds.push($(this).attr("value"));
            });


            $.ajax({
                type: 'POST',
                url: "/EditSet/AddQuestionsToSet",
                data: { setid: EditSet.GetSetId(), questionIds: selectedQuestionIds },
                success: function (data) {
                    if (data.Status === true) {

                        $("#addQuestionsOutputInfo").text("Die Frage(n) wurden erfolgreich hinzugefügt und das Lernset gespeichert. Wenn du möchtest, kannst du weitere Fragen hinzufügen.");
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
                        $("#addQuestionsOutputInfo").removeClass("alert-success").addClass("alert-warning");
                        $("#addQuestionsOutputInfo").text("Speichern fehlgeschlagen");
                    }
                },
                dataType: 'json',
                async: false
            });



            $("#addQuestionsOutputInfo").fadeIn();
            $("#resultQuestions").empty();
            $('#addMarkedQuestionsToSetAndSave').hide();
            $("#addMarkedQuestionsToSetAndSave").prop("disabled", false);
            $("#addQuestionSearchField").val("");
            $("#questionSearchResults").hide();

        });
});

var options = {
    callback: function () {
        $("#resultQuestions").empty();
        $("#addQuestionsOutputInfo").hide();
        $.post("/EditSet/Search",
            { setId: EditSet.GetSetId(), term: $("#addQuestionSearchField").val() },
            function(data) {

                if (data.Questions.length > 0) {
                    $("#questionSearchResults").fadeIn();
                    $('#resultQuestions').fadeIn();
                    for (var i = 0; i < data.Questions.length; i++) {
                        var questionRowHtml =
                            "<div class='questionRows row'> " +
                                "<div class='col-xs-1 questionSearchResultCheckbox'> " +
                                    "<input type='checkbox' value=" + data.Questions[i].Id + " />" +
                                "</div>"+
                                "<div class='col-xs-2 questionSearchResultImage'>" +
                                    "<image src=" + data.Questions[i].ImageUrl + " class='previewImage' >" +
                                "</div>" +
                                "<div class='col-xs-9 xxs-stack questionSearchResultText'>" +
                                    data.Questions[i].question +
                                    "<div class='correctAnswer'>Richtige Antwort: " + data.Questions[i].correctAnswer + "</div>" +
                                    "<p class='linkQuestion'>" +
                                        "<a target='_blank' href=" + data.Questions[i].QuestionUrl + ">zur Frage <i class='fa fa-external-link'></i></a>" +
                                    "</p>" +
                                "</div>" +
                            "</div>";
                        $("#resultQuestions").append(questionRowHtml);
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
        $('#addMarkedQuestionsToSetAndSave').fadeIn();
    } else {

        var checkedCount = $("#resultQuestions input:checkbox")
            .filter(function (index, input) { return $(input).is(":checked") })
            .length;

        if (checkedCount === 0)
            $('#addMarkedQuestionsToSetAndSave').hide();
    }

}

function disableSubmitButtonSouldBePressedEnter() {
    $('body').keypress(function (e) {
        if (e.keyCode === 13 && $("#addQuestionSearchField").is(":focus")) {
            event.preventDefault();
            return false;
        }
    });
}

function hiding() {
    $("#addQuestionsOutputInfo").hide();
    $("#questionSearchResults").hide();
    $('#resultQuestions').hide();
    $('#addMarkedQuestionsToSetAndSave').hide();
}

