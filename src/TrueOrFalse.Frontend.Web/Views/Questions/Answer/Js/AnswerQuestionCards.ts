class AnswerQuestionCards {
    constructor() {
        var children = $("#ParentsChildrenTopics #contentRecommendation").children();
        this.hideChildren(children);

        children.length > 5 ? $("#MoreParentsAndChildrens").removeClass("hide") : $("#MoreParentsAndChildrens").addClass("hide");

        $("#MoreParentsAndChildrens").on("click", () => {
            if ($("#ParentsChildrenTopics #contentRecommendation").children().eq(6).hasClass("hide"))
                $("#ParentsChildrenTopics #contentRecommendation").children().removeClass("hide");
            else
                this.hideChildren(children);
        });
    }
    private hideChildren(children) {
        for (var i = 0; i < children.length; i++) {
            if (i > 5) {
                $("#ParentsChildrenTopics #contentRecommendation").children().eq(i).addClass("hide");

            }
        }
    }
}
$(() => {

    new AnswerQuestionCards();
});