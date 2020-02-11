class AnswerQuestionCards {
    constructor() {
        var children = $("#ParentsChildrenTopics #contentRecommendation").children();
        this.hideChildren(children);
        $("#MoreParentsAndChildrens").addClass("hide");

        $(window).on("resize",
            () => {
                if ($(window).width() <= 750)
                    $(".singleCatFullWidth").children().eq(0).removeClass("flex");
                else
                    $(".singleCatFullWidth").children().eq(0).addClass("flex");

            });

        if (children.length > 5) {
            $("#MoreParentsAndChildrens").removeClass("hide").css("cursor", "pointer");
            $("#MoreParentsAndChildrens").children().eq(0).text("Alle Themen anzeigen (" + children.length + ")");

        }

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