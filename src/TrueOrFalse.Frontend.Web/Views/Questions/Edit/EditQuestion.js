$(function () {
    $('#Question').defaultText("Bitte geben Sie eine Frage ein.");
    $('#Answer').defaultText("Antwort eingeben.");
    $('#Description').defaultText("Erklärung der Antwort und Quellen.");

    for (var i = 0; i <= 5; i++) {
        $("#cat" + i).autocomplete({
            source: '/Api/Category/ByName',
            minLength: 1
        });
    }
});