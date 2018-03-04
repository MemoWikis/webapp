
class SquareWishKnowledge {

    constructor() {

        //var allSquares = $(".square-wish-knowledge");

        //allSquares.on("mouseover", function(eventObject, args) {
        //    console.log(event);                                              //event.srcElement.attributes[1].nodeValue
        //    $(this).tooltip("show");
          
           
        //});

        $.ajax({
            type: "POST",
            url: "WishKnowledgeInTheBoxModel/GetObjectQuestionKnowledge",    // Views/Categories/Detail/Partials/
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
            },
            success: function (result) {
                alert("We returned: " + result);
            }
        });
    }
}

jQuery(document).ready(() => {
    var main: SquareWishKnowledge = new SquareWishKnowledge();
});