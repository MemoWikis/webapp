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
        var self = this;
        $.ajax({
            type: 'POST',
            url: "/Dates/Copy/",
            data: { sourceDateId: sourceDateId },
            cache: false,
            success: function (result) {
                //window.alert("Termin wurde übernommen, bitte Seite neu laden. ID: " + result.CopiedDateId.toString() + ", dann ID " + result.PrecedingDateId.toString());
                self.RenderCopiedDate(result.CopiedDateId, result.PrecedingDateId);
            },
            error: function (e) {
                console.log(e);
            }
        });
    }

    RenderCopiedDate(copiedDateId : number, precedingDateId : number) {
        //precedingDateId is Id of date after which the newly copied date should be inserted; 0 if newly copied date is first date in list
        //$("hallo1").prependTo("#allDateRows");
        //$("hallo2").insertAfter('[data-date-id="22"]');
        //$("<p>hallo3</p>").prependTo("#allDateRows");
        //$("<p>hallo4</p>").insertAfter('[data-date-id="22"]');
        if (precedingDateId == 0) {
            $("<div>hier kommt date-id" + copiedDateId + "</div>").prependTo("#allDateRows");
        } else {
            $("<div>hier kommt date-id" + copiedDateId + "</div>").insertAfter('[data-date-id="' + precedingDateId + '"]');
        }
        //$("#startingOwnDates")
        //    .empty()
        //    .animate({ opacity: 0.00 }, 0)
        //    .append(copiedDateId)
        //    .append("-kommt nach-")
        //    .append(precedingDateId)
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