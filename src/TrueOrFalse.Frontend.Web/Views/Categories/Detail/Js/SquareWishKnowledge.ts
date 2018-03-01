
class Main {

    constructor() {

        var allSquares = $(".square-wish-knowledge");

        allSquares.on("mouseover", function(eventObject, args) {
            console.log(event);                                              //event.srcElement.attributes[1].nodeValue
            $(this).tooltip("show");
          
           
        });

      
    }
}

jQuery(document).ready(() => {
    var main: Main = new Main();
});