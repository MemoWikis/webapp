class Comments
{
    constructor() {
        var self = this;
        $("#btnSaveComment").click((e) => this.SaveComment(e));
        $(".btnAnswerComment").click(function(e) { self.ShowAddAnswer(e, this); });
    }

    SaveComment(e : BaseJQueryEventObject) {

        e.preventDefault();

        var params = {
            questionId: window.questionId,
            text: $("#txtNewComment").val()
        }

        var txtNewComment = $("#txtNewComment");
        $("#saveCommentSpinner").show();
        txtNewComment.hide();

        $.post("/AnswerComments/SaveComment", params, function(data) {
            $("#comments").append(data);

            txtNewComment.attr("placeholder", "Dein Kommentar wurde gespeichert.");
            txtNewComment.val("");
            $("#saveCommentSpinner").hide();
            txtNewComment.show();
        });
    }

    ShowAddAnswer(e: BaseJQueryEventObject, buttonElem : JQuery) {

        e.preventDefault();

        var self = this;
        buttonElem = $(buttonElem);

        $.post("/AnswerComments/GetAnswerHtml", function (data) {
            buttonElem.parent().hide();

            var html = $(data);
            var btnSaveAnswer = $(html.find(".saveAnswer")[0]);
            var parentContainer = $($(buttonElem).parents(".panel")[0]);
            btnSaveAnswer.click(function(e) {
                self.SaveAnswer(e, parentContainer, html, buttonElem.data("comment-id"));
            });
            parentContainer.append(html);

        });
    }

    SaveAnswer(e: BaseJQueryEventObject, parentContainer: JQuery, divAnswerEdit : JQuery, commentId : number) {

        e.preventDefault();

        var params = {
            commentId: commentId,
            text: $(divAnswerEdit.find("textarea")[0]).val()
    }

        divAnswerEdit.remove();

        var progress =
            $("<div class='panel-body' style='position: relative;'>" +
                "<div class='col-lg-offset-2 col-lg-10' style='height: 50px;'>" +
                    "<i class='fa fa-spinner fa-spin'></i>" +
                "</div>" +
              "</div>");

        parentContainer.append(progress);

        $.post("/AnswerComments/SaveAnswer", params, function(data) {
            progress.hide();
            parentContainer.append(data);
        });
    }
}

$(function () {
    new Comments();
});