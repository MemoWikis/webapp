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
    }
}