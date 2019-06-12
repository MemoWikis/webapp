class SquareWishKnowledge {

    constructor(page: CategoryPage) {

        if (!$("#knowledgeAsABox").html())
            return;

        $.post("/Category/WishKnowledgeInTheBox", { categoryId: page.categoryId })
            .done((result) => {

                $(".wishKnowledgeTemplate").html(result);
                console.log("We returned: " + result);

                var allSquares = $(".square-wish-knowledge");
                allSquares.on("mouseover", function () {
                    console.log(event);
                    $(this).tooltip("show");
                });

            })
            .fail((XMLHttpRequest, textStatus, errorThrown) => {
                console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown
                );
            });
    }
}