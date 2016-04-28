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
            error: function (e) {
                console.log(e);
            }
        });
    }

    CopyDate(sourceDateId) {
        $.ajax({
            type: 'POST',
            url: "/Dates/Copy/",
            data: { sourceDateId: sourceDateId },
            cache: false,
            success: function (result) {
                window.alert("Termin wurde übernommen, bitte Seite neu laden. ID: " + result.DateId.toString());
                //this.RenderCopiedDate(result.DateId.toString());
            },
            error: function (e) {
                console.log(e);
            }
        });
    }

    RenderCopiedDate(copiedDateId) {
        //var dateNodes = document.getElementsByClassName("rowBase date-row");
        //$("#startingOwnDates")
        //    .empty()
        //    .animate({ opacity: 0.00 }, 0)
        //    .append(copiedDateId)
        //    .append("ichwarhier")
        //    .animate({ opacity: 1.00 }, 600);
        //$(".show-tooltip").tooltip();

        //$.get("/Dates/RenderCopiedDate/" + copiedDateId,
        //    htmlResult => {
        //        $("#startingOwnDates")
        //            .empty()
        //            .animate({ opacity: 0.00 }, 0)
        //            .append(htmlResult)
        //            .append("ichwarhier")
        //            .animate({ opacity: 1.00 }, 600);

        //        $(".show-tooltip").tooltip();
        //    });
    }
}