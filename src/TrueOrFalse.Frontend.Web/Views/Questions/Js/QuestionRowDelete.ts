/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class QuestionDelete {
    constructor() {

        var questionIdToDelete;
        $(function () {
            $('a[href*=#modalDelete]').click(function (e) {
                questionIdToDelete = $(this).attr("data-questionId");
                populateDeleteQuestionId(questionIdToDelete);
                e.preventDefault();
            });

            $('#btnCloseQuestionDelete').click(function (e) {
                $('#modalDelete').modal('hide');
                e.preventDefault();
            });

            $('#confirmQuestionDelete').click(function (e) {
                deleteQuestion(questionIdToDelete);
                $('#modalDelete').modal('hide');
                e.preventDefault();
            });
        });

        function populateDeleteQuestionId(questionId) {
            $.ajax({
                type: 'POST',
                url: "/Questions/DeleteDetails/" + questionId,
                cache: false,
                success: function (result) {
                    if (result.canNotBeDeleted) {
                        $("#questionDeleteCanDelete").hide();
                        $("#questionDeleteCanNotDelete").show();
                        $("#confirmQuestionDelete").hide();
                        $("#questionDeleteCanNotDelete").html(result.canNotBeDeletedReason);
                        $("#btnCloseQuestionDelete").html("Schließen");
                    } else {
                        $("#questionDeleteCanDelete").show();
                        $("#questionDeleteCanNotDelete").hide();
                        $("#confirmQuestionDelete").show();
                        $("#spanQuestionTitle").html(result.questionTitle.toString());
                        $("#btnCloseQuestionDelete").html("Abbrechen");
                    }
                    
                },
                error: function (e) {
                    console.log(e);
                    window.alert("Ein Fehler ist aufgetreten");
                }
            });
        }

        function deleteQuestion(questionId) {
            $.ajax({
                type: 'POST',
                url: "/Questions/Delete/" + questionId,
                cache: false,
                success: function () { window.location.reload(); },
                error: function (e) {
                    console.log(e);
                    window.alert("Ein Fehler ist aufgetreten");
                }
            });
        }
        
    }
}

