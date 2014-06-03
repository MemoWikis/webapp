var Comments = (function () {
    function Comments() {
        var _this = this;
        $("#btnSaveComment").click(function (e) {
            return _this.AddComment(e);
        });
    }
    Comments.prototype.AddComment = function (e) {
        e.preventDefault();

        var params = {
            questionId: window.questionId,
            text: $("#txtNewComment").val()
        };

        var txtNewComment = $("#txtNewComment");
        $("#saveCommentSpinner").show();
        txtNewComment.hide();

        $.post("/AnswerComments/AddComment", params, function (data) {
            $("#comments").append(data);

            txtNewComment.attr("placeholder", "Dein Kommentar wurde gespeichert.");
            txtNewComment.val("");
            $("#saveCommentSpinner").hide();
            txtNewComment.show();
        });
    };
    return Comments;
})();

$(function () {
    new Comments();
});
//# sourceMappingURL=Comments.js.map
