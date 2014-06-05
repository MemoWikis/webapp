var Comments = (function () {
    function Comments() {
        var _this = this;
        var self = this;
        $("#btnSaveComment").click(function (e) {
            return _this.AddComment(e);
        });
        $(".btnAnswerComment").click(function (e) {
            self.ShowAddAnswer(e, this);
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

    Comments.prototype.ShowAddAnswer = function (e, buttonElem) {
        e.preventDefault();

        $.post("/AnswerComments/GetAnswerHtml", function (data) {
            $(buttonElem).parent().hide();
            $($(buttonElem).parents(".panel")[0]).append(data);
        });
    };
    return Comments;
})();

$(function () {
    new Comments();
});
//# sourceMappingURL=Comments.js.map
