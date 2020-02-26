class AnswerQuestionCards {
    private  CounterCards :Number = 3; 
    constructor() {
        var children = $("#ParentsChildrenTopics").children();
        $(window).on("resize",
            () => {
                if ($(document).outerWidth() <= 463)
                    this.CounterCards = 1;
                else if ($(document).outerWidth() <= 754)
                    this.CounterCards = 2;
                else
                    this.CounterCards = 3;
                this.hideChildren(children);
            });
        
        
        $("#MoreParentsAndChildrens").addClass("hide");

        if (children.length > 5) {
            $("#MoreParentsAndChildrens").removeClass("hide").css("cursor", "pointer");
            $("#MoreParentsAndChildrens").children().eq(0).text("Alle Themen anzeigen (" + children.length + ")");
        }

        $("#MoreParentsAndChildrens").on("click", () => {
            if ($("#ParentsChildrenTopics").children().eq(6).hasClass("hide"))
                $("#ParentsChildrenTopics").children().removeClass("hide");
            else
                this.hideChildren(children);
        });
    }
    private hideChildren(children) {

        for (var i = 0; i < children.length; i++) {
            if (i > this.CounterCards) {
                $("#ParentsChildrenTopics").children().eq(i).addClass("hide");

            }
        }
    }
}
$(() => {
    new AnswerQuestionCards();
});