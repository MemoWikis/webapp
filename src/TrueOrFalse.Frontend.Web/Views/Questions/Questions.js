/*** FILTER ****/
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
});

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

/*** SLIDER ****/


