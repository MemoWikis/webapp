
class Main {

    constructor() {

        var allSquares = $(".square-wish-knowledge");
        allSquares.on("click", function(eventObject, args) {
            console.log(event.srcElement.attributes[1].nodeValue);
            console.log($(".square-wish-knowledge").length);

            $("#questionText0").fadeIn();
        });

        allSquares.tooltip();
    }
}

jQuery(document).ready(() => {
    var main: Main = new Main();
});