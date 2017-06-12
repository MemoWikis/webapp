$(document).ready(function () {
    (<any>$('#tableSetStats')).DataTable({
        "language": {
            "decimal": ",",
            "thousands": "."
        }
    });
});