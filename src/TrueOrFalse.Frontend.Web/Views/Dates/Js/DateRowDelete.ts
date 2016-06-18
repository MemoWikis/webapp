/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class DateRowDelete {

    constructor() {
        var self = this;
        var dateIdToDelete;
        $("#allDateRows").on("click", 'a[href*=#modalDelete]', function () {
            $("#spanDeleteDateInfo").html("... (wird geladen) ...");
            dateIdToDelete = $(this).attr("data-dateId");
            self.PopulatModal(dateIdToDelete);
        });

        $("#modalDelete").on("click",'#btnCloseDateDelete', function (e) {
            e.preventDefault();
            $('#modalDelete').modal('hide');
        });

        $("#modalDelete").on("click",'#btnConfirmDateDelete', function (e) {
            e.preventDefault();
            self.DeleteDate(dateIdToDelete);
            $('#modalDelete').modal('hide');
        });        
    }


    PopulatModal(dateId) {
        $.ajax({
            type: 'POST',
            url: "/Dates/DeleteDetails/" + dateId,
            cache: false,
            success: function (result) {
                $("#spanDeleteDateInfo").html(result.DateInfo.toString());
            },
            error: function (e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten.");
            }
        });
    }

    DeleteDate(dateId) {
        $.ajax({
            type: 'POST',
            url: "/Dates/Delete/" + dateId,
            cache: false,
            success: function() {
                DateRow.HideRow(dateId);
            },
            error: function (e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten.");
            }
        });
    }
}