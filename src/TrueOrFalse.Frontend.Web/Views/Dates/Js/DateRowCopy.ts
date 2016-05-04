/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />

declare var drawKnowledgeCharts: any;
declare var dateRowDelete: any;

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
        $.get("/Dates/RenderCopiedDate/" + copiedDateId,
            function (htmlResult) {
                //if box "Du hast keine aktuellen Termine" still there, hide it!
                $("#noOwnCurrentDatesInfo").hide();

                //insert copied date
                if (precedingDateId == 0) {
                    $("#allDateRows").prepend(htmlResult);
                } else {
                    $('[data-date-id="' + precedingDateId + '"]').after(htmlResult);
                }
                drawKnowledgeCharts();
                InitTooltips();

                //animate newly inserted date
                var bgOrg = $('[data-date-id="' + copiedDateId + '"]').css("background-color");
                $('[data-date-id="' + copiedDateId + '"]')
                    .animate({ backgroundColor: "#afd534", opacity: 0.00 }, 0)
                    .animate({ opacity: 1.00}, 900)
                    .animate({ backgroundColor: bgOrg}, 900);
            });

    }
}