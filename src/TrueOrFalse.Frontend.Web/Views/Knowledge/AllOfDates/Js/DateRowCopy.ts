/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />

declare var drawKnowledgeCharts: any;
declare var dateRowDelete: any;
declare function ga(text: string, text2: string, text3?: string, text4?: string, text5?: string): any;

class DateRowCopy {

    constructor() {

        var self = this;
        var dateIdToCopy;
        
        $('a[href*=#modalCopyDate]').click(function () {
            dateIdToCopy = $(this).attr("data-dateId");
            self.PopulatModal(dateIdToCopy);
            ga('send', 'event', 'CopyDate', 'OpenModalCopyDate', 'OpenModalCopyDate');
        });

        $('#btnCloseDateCopy').click(function (e) {
            e.preventDefault();
            $('#modalCopyDate').modal('hide');
        });

        $('#btnConfirmDateCopy').click(function (e) {
            e.preventDefault();
            self.CopyDate(dateIdToCopy);
            $('#modalCopyDate').modal('hide');
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
                self.RenderCopiedDate(sourceDateId, result.CopiedDateId, result.PrecedingDateId);
            },
            error: function (e) {
                console.log(e);
            }
        });
    }

    RenderCopiedDate(sourceDateId : number, copiedDateId : number, precedingDateId : number) {
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
                if (!Utils.IsScrolledIntoView('[data-date-id="' + copiedDateId + '"]')) {
                    $('html, body').animate({scrollTop: $('[data-date-id="' + copiedDateId + '"]').offset().top - 80}, 600);
                }

                //animate newly inserted date
                var bgOrg = $('[data-date-id="' + copiedDateId + '"]').css("background-color");
                $('[data-date-id="' + copiedDateId + '"]')
                    .animate({ backgroundColor: "#afd534", opacity: 0.00 }, 0)
                    .animate({ opacity: 1.00}, 900)
                    .animate({ backgroundColor: bgOrg}, 900);

                var newAmount = parseInt($('[data-date-id="' + sourceDateId + '"] .numberOfTimesCopied').html()) + 1;
                Utils.SetElementValue('[data-date-id="' + sourceDateId + '"] .numberOfTimesCopied', newAmount.toString());
            });

    }
}