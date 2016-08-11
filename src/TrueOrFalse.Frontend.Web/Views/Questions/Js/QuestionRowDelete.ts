enum QuestionRowDeleteSourcePage {
    QuestionRow,
    QuestionDetail
} 

class QuestionRowDelete {
    
    constructor(sourcePage: QuestionRowDeleteSourcePage) {

        var questionIdToDelete;
        $(function () {
            $('a[href*=#modalDeleteQuestion]').click(function (e) {
                questionIdToDelete = $(this).attr("data-questionId");
                populateDeleteQuestionId(questionIdToDelete);
                e.preventDefault();
            });

            $('#btnCloseQuestionDelete').click(function (e) {
                $('#modalDeleteQuestion').modal('hide');
                e.preventDefault();
            });

            $('#confirmQuestionDelete').click(function (e) {
                deleteQuestion(questionIdToDelete, sourcePage);
                $('#modalDeleteQuestion').modal('hide');
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

        function deleteQuestion(questionId, sourcePage: QuestionRowDeleteSourcePage) {
            $.ajax({
                type: 'POST',
                url: "/Questions/Delete/" + questionId,
                cache: false,
                success: function (e) {
                    if (sourcePage == QuestionRowDeleteSourcePage.QuestionRow)
                        window.location.reload();
                    if (sourcePage == QuestionRowDeleteSourcePage.QuestionDetail)
                        window.location.href = "/Fragen/Meine";
                },
                error: function (e) {
                    console.log(e);
                    window.alert("Ein Fehler ist aufgetreten");
                }
            });
        }
        
    }
}

