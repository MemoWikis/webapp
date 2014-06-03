class Comments
{
    constructor() {
        $("#btnSaveComment").click((e) => this.AddComment(e));
    }

    AddComment(e : BaseJQueryEventObject) {

        e.preventDefault();

        var params = {
            questionId: window.questionId,
            text: $("#txtNewComment").val()
        }

        var txtNewComment = $("#txtNewComment");
        $("#saveCommentSpinner").show();
        txtNewComment.hide();

        $.post("/AnswerComments/AddComment", params, function(data) {
            $("#comments").append(data);

            txtNewComment.attr("placeholder", "Dein Kommentar wurde gespeichert.");
            txtNewComment.val("");
            $("#saveCommentSpinner").hide();
            txtNewComment.show();
        });
    }
}


$(function () {
    new Comments();
});