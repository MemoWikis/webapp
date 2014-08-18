/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class QuestionDelete {
    constructor() {

        var questionIdToDelete;
        $(function () {
            $('a[href*=#modalDelete]').click(function () {
                questionIdToDelete = $(this).attr("data-questionId");
                populateDeleteQuestionId(questionIdToDelete);
            });

            $('#btnCloseQuestionDelete').click(function () {
                $('#modalDelete').modal('hide');
            });

            $('#confirmQuestionDelete').click(function () {
                deleteQuestion(questionIdToDelete);
                $('#modalDelete').modal('hide');
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
                success: function () { window.location.reload(); },
                error: function (result) {
                    alert("Ein Fehler ist aufgetreten");
                }
            });
        }
        
    }
}

