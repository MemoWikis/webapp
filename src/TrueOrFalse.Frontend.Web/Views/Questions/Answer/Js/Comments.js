var Comments = (function () {
    function Comments() {
        var _this = this;
        $("#btnSaveComment").click(function () {
            return _this.AddComment();
        });
    }
    Comments.prototype.AddComment = function () {
        var params = {
            questionId: window.questionId,
            text: $("#txtNewComment").val()
        };

        $.post("/AnswerComments/AddComment", params, function (data) {
            $("#comments").append(data);
        });
    };
    return Comments;
})();

$(function () {
    new Comments();
});
//# sourceMappingURL=Comments.js.map
