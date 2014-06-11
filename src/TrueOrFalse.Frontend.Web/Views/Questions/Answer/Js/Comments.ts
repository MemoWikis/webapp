class Comments
{
    constructor() {
        var self = this;
        $("#btnSaveComment").click((e) => this.SaveComment(e));
        $("#btnImprove").click((e) => this.SaveImproveComment(e));
        this.RegisterBtnAnswerComment($(window));
    }

    RegisterBtnAnswerComment(parent : JQuery) {
        var self = this;
        $(".btnAnswerComment").click(function (e) { self.ShowAddAnswer(e, this); });        
    }

    SaveImproveComment(e: BaseJQueryEventObject) {

        window.scrollTo(0, document.body.scrollHeight);

        var params = {
            questionId: window.questionId,
            text: $("#txtImproveBecause").val(),
            typeImprovement: true,
            typeKeys: $("input:checked[name='ckbImprove']")
                        .map(function () { return $(this).val(); })
                        .get().join(", ")
                
    }

        this.SaveCommentJson(e, params);

        $("#modalImprove").modal('hide');
    }

    SaveComment(e: BaseJQueryEventObject) {
        var params = {
            questionId: window.questionId,
            text: $("#txtNewComment").val()
        }

        this.SaveCommentJson(e, params);
    }

    SaveCommentJson(e: BaseJQueryEventObject, params : {}) {

        e.preventDefault();
        var self = this;

        var txtNewComment = $("#txtNewComment");
        $("#saveCommentSpinner").show();
        txtNewComment.hide();

        $.post("/AnswerComments/SaveComment", params, function (data) {
            $("#comments").append(data);

            txtNewComment.attr("placeholder", "Dein Kommentar wurde gespeichert.");
            txtNewComment.val("");
            $("#saveCommentSpinner").hide();
            txtNewComment.show();
            self.RegisterBtnAnswerComment(txtNewComment);
        });
        
    }

    ShowAddAnswer(e: BaseJQueryEventObject, buttonElem : JQuery) {

        e.preventDefault();

        var self = this;
        buttonElem = $(buttonElem);

        $.post("/AnswerComments/GetAnswerHtml", function (data) {

            var html = $(data);
            var btnSaveAnswer = $(html.find(".saveAnswer")[0]);
            var parentContainer = $($(buttonElem).parents(".panel")[0]);

            var answerRow = $(buttonElem.parents(".panel-body")[0]);
            answerRow.remove();

            btnSaveAnswer.click(function(e) {
                self.SaveAnswer(e, parentContainer, html, buttonElem.data("comment-id"), answerRow);
            });
            parentContainer.append(html);

        });
    }

    SaveAnswer(
        e: BaseJQueryEventObject,
        parentContainer: JQuery,
        divAnswerEdit: JQuery,
        commentId: number,
        answerRow : JQuery) {

        e.preventDefault();
        var self = this;

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
            parentContainer.append(answerRow);
            self.RegisterBtnAnswerComment(answerRow);
        });
    }
}

$(function () {
    new Comments();
});