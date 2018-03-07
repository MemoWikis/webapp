
class SquareWishKnowledge {

    constructor(page: CategoryPage) {


        $.post("/Category/WishKnowledgeInTheBox", { categoryId: page.CategoryId })                      //
            .done((result) => {

                $(".wishKnowledgeTemplate").html(result);
                console.log("We returned: " + result);


                var allSquares = $(".square-wish-knowledge");
                allSquares.on("mouseover", function () {
                    console.log(event);                                              //event.srcElement.attributes[1].nodeValue
                    $(this).tooltip("show");
                });

            })
            .fail((XMLHttpRequest, textStatus, errorThrown) => {
                console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown
                );
            });
    }
}