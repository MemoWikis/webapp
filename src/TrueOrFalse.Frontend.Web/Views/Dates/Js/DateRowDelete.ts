/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class DateRowDelete {
    constructor() {

        var self = this;
        var dateIdToDelete;
        
        $('a[href*=#modalDelete]').click(function () {
            dateIdToDelete = $(this).attr("data-dateId");
            self.PopulatModal(dateIdToDelete);
        });

        $('#btnCloseDateDelete').click(function () {
            $('#modalDelete').modal('hide');
        });

        $('#confirmDateDelete').click(function () {
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
                $("#spanDateInfo").html(result.DateInfo.toString());
            },
            error: function () {
                window.alert("Ein Fehler ist aufgetreten");
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
            error: function (result) {
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    }
}

