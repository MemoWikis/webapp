class WishKnowledgeContent {

    constructor() {

        $("#btnShowAllWishKnowledgeContent").click(function (e) {
            e.preventDefault();
            $("#wishKnowledgeMore").show();
            $("#btnShowAllWishKnowledgeContent").hide();
            $("#btnShowLessWishKnowledgeContent").show();
        });

        $("#btnShowLessWishKnowledgeContent").click(function(e) {
            e.preventDefault();
            $("#wishKnowledgeMore").hide();
            $("#btnShowAllWishKnowledgeContent").show();
            $("#btnShowLessWishKnowledgeContent").hide();
        });

        $(".link-to-topic").on("click",
            function (e) {
                e.preventDefault();
                window.location.replace("/Kategorien/Schule/" + $(this).attr("data-category") + "?openTab=learningTab");
                console.log($(this).attr("data-category"));
                debugger;

            });
    }
}