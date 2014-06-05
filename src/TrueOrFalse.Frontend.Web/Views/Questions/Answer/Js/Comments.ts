class Comments
{
    constructor() {
        var self = this;
        $("#btnSaveComment").click((e) => this.AddComment(e));
        $(".btnAnswerComment").click(function(e) { self.ShowAddAnswer(e, this); });
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

    ShowAddAnswer(e: BaseJQueryEventObject, buttonElem : JQuery) {

        e.preventDefault();

        $.post("/AnswerComments/GetAnswerHtml", function (data) {
            $(buttonElem).parent().hide();
            $($(buttonElem).parents(".panel")[0]).append(data);
        });
    }
}

$(function () {
    new Comments();
});