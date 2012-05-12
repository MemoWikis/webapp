
$(function () {
    $('a[href*=#modalDelete]').click(function () {
        populateDeleteQuestionId($(this).attr("data-questionId"));
    });
});

function populateDeleteQuestionId(questionId) {
    $.ajax({
        type: 'POST',
        url: "/Questions/GetQuestionDeleteDetails/" + questionId,
        cache: false,
        success: function (result) {
            $("#spanQuestionTitle").innerText = result.questionTitle;
            if (result.correct) {
            } else {
            };
        },
        error: function (result) {
            alert(result);
        }
    });

}