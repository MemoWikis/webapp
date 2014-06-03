class Comments
{
    constructor() {
        $("#btnSaveComment").click(() => this.AddComment());
    }

    AddComment() {

        var params = {
            questionId: window.questionId,
            text: $("#txtNewComment").val()
        }

        $.post("/AnswerComments/AddComment", params, function(data) {
            $("#comments").append(data);
        });
    }
}


$(function () {
    new Comments();
});