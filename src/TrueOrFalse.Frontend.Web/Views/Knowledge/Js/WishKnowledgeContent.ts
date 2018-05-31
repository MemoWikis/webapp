class WishKnowledgeContent {

    constructor() {

        $("#btnShowAllWishKnowledgeContent").click(function (e) {
            e.preventDefault();
            $("#wishKnowledgeMore").show();
            $("#btnShowAllWishKnowledgeContent").hide();
            $("#btnShowLessWishKnowledgeContent").show();
        });

        $("#btnShowLessWishKnowledgeContent").click(function (e) {
            e.preventDefault();
            $("#wishKnowledgeMore").hide();
            $("#btnShowAllWishKnowledgeContent").show();
            $("#btnShowLessWishKnowledgeContent").hide();
        });

        $(".fa-heart").on("click",
            function (e) {
                e.preventDefault();

                if ($(this).attr("data-set") != null) {
                    SetsApi.Unpin($(this).attr("data-id"));
                    $(this).closest(".row").remove();
                    return;
                }

                CategoryApi.Unpin($(this).attr("data-id"));
                $(this).closest(".row").remove();
            });

        $(".link-to-topic").on("click",
            function (e) {
                e.preventDefault();
                window.location.replace("/Kategorien/Schule/" + $(this).attr("data-category") + "?openTab=learningTab");
            });

        $(".link-to-learnset").on("click",
            function(e) {
                e.preventDefault();
                window.location.replace("/Fragesatz/Lernen/" + $(this).attr("data-setId"));
            });
    }
}
