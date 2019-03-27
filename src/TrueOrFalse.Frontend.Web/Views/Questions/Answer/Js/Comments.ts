class Comments
{
    constructor() {
        var self = this;
        $("#btnSaveComment").click((e) => this.SaveComment(e));
        $("#btnImprove").click((e) => this.SaveImproveComment(e));
        $("#btnShouldDelete").click((e) => this.SaveDeleteComent(e));
        $("#saveCommentSpinner").hide();
        $(document).on("click", ".btnAnswerComment", function (e) {
            self.ShowAddAnswer(e, this);
        });        
        $(document).on("click", ".btnMarkAsSettled", function (e) {
            e.preventDefault();
            self.MarkAsSettled(this);
        });
        $(document).on("click", ".btnMarkAsUnsettled", function (e) {
            e.preventDefault();
            self.MarkAsUnsettled(this);
        });
        
        $(document).on("click", ".showAllAnswersInclSettled", function (e) {
            e.preventDefault();
            self.showAllAnswersInclSettled(this);
        });

        $(document).on("click", "#showAllCommentsInclSettled", function (e) {
            e.preventDefault();
            self.showAllCommentsInclSettled(this);
        });

    }

    SaveDeleteComent(e: BaseJQueryEventObject) {
        window.scrollTo(0, window.document.body.scrollHeight);

        var params = {
            questionId: AnswerQuestion.GetQuestionId(),
            text: $("#txtDeleteBecause").val(),
            typeRemove: true,
            typeKeys: $("input:checked[name='ckbDelete']")
                .map(function () { return $(this).val(); })
                .get().join(", ")
        }

        this.SaveCommentJson(e, params);

        $("#modalDelete").modal('hide');
    }

    SaveImproveComment(e: BaseJQueryEventObject) {

        window.scrollTo(0, window.document.body.scrollHeight);

        var params = {
            questionId: AnswerQuestion.GetQuestionId(),
            text: $("#txtImproveBecause").val(),
            typeImprovement: true,
            typeKeys: $("input:checked[name='ckbImprove']")
                .map(function () { return $(this).val(); })
                .get().join(", ")        
        }

        this.SaveCommentJson(e, params);

        $("#modalQuestionFlagImprove").modal('hide');
    }

    SaveComment(e: BaseJQueryEventObject) {
        var params = {
            questionId: AnswerQuestion.GetQuestionId(),
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
                self.SaveAnswer(e, parentContainer, html, <any>buttonElem.data("comment-id"), answerRow);
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
        });
    }

    MarkAsSettled(buttonElem: JQuery) {
        buttonElem = $(buttonElem);
        var commentId = buttonElem.data("comment-id");
        $.ajax({
            type: 'POST',
            url: "/AnswerComments/MarkCommentAsSettled",
            data: { commentId: commentId },
            cache: false,
            success(e) {
                buttonElem.hide();
                buttonElem.parent().find("[data-type=btn-markAsUnsettled]").show();
            },
            error(e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    }

    MarkAsUnsettled(buttonElem: JQuery) {
        buttonElem = $(buttonElem);
        var commentId = buttonElem.data("comment-id");
        $.ajax({
            type: 'POST',
            url: "/AnswerComments/MarkCommentAsUnsettled",
            data: { commentId: commentId },
            cache: false,
            success(e) {
                buttonElem.hide();
                buttonElem.parent().find("[data-type=btn-markAsSettled]").show();
            },
            error(e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    }

    showAllAnswersInclSettled(buttonElem: JQuery) {
        buttonElem = $(buttonElem);
        var commentId = buttonElem.data("comment-id");
        $.ajax({
            type: 'POST',
            url: "/AnswerComments/GetAllAnswersInclSettledHtml",
            data: { commentId: commentId },
            cache: false,
            success(data) {
                var commentDiv = buttonElem.parents(".comment");
                commentDiv.html(data);
                commentDiv.animate({ opacity: 0.00 }, 0)
                    .animate({ opacity: 1 }, 800);
            },
            error(e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten.");
            }
        });
    }

    showAllCommentsInclSettled(buttonElem: JQuery) {
        buttonElem = $(buttonElem);
        var questionId = buttonElem.data("question-id");
        console.log("showing comments for question " + questionId);
        $.ajax({
            type: 'POST',
            url: "/AnswerComments/GetAllCommentsInclSettledHtml",
            data: { questionId: questionId },
            cache: false,
            success(data) {
                var commentsDiv = buttonElem.parents("#comments");
                commentsDiv.html(data);
                console.log(data);
                $(".commentIsSettled").animate({ opacity: 0.00 }, 0)
                    .animate({ opacity: 1 }, 1200);
            },
            error(e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten.");
            }
        });
    }
}

$(function () {
    new Comments();
});