/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class DateRowCopy {
    constructor() {

        var self = this;
        var dateIdToCopy;
        
        $('a[href*=#modalCopy]').click(function () {
            dateIdToCopy = $(this).attr("data-dateId");
            self.PopulatModal(dateIdToCopy);
        });

        $('#btnCloseDateCopy').click(function () {
            $('#modalCopy').modal('hide');
        });

        $('#btnConfirmDateCopy').click(function () {
            self.CopyDate(dateIdToCopy);
            $('#modalCopy').modal('hide');
        });        
    }

    PopulatModal(dateId) {
        $.ajax({
            type: 'POST',
            url: "/Dates/CopyDetails/" + dateId,
            cache: false,
            success: function (result) {
                $("#spanCopyDateInfo").html(result.DateInfo.toString());
                $("#spanCopyDateOwner").html(result.DateOwner.toString());
            },
            error: function () {
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    }

    CopyDate(dateId) {
        $.ajax({
            type: 'POST',
            url: "/Dates/Copy/" + dateId,
            cache: false,
            success: function () {
                window.alert("Hat geklappt");
            },
            error: function (result) {
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    }
}