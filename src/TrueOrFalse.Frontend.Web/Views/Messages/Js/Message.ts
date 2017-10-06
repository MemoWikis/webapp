class Message {

    constructor() {

        var fnSetValue = function(selector: string, newValue: any) {
            $(selector)
                .text(newValue)
                .animate({ opacity: 0.25 }, 100)
                .animate({ opacity: 1.00 }, 500);
        };

        $("[data-messageId]").each(function() {

            var row = $(this);
            var msgId = row.attr("data-messageId");

            row
                .find("a.markAsRead")
                .click(function(e) {
                    e.preventDefault();
                    $(this).parent().hide();
                    $(this).parent().parent().find(".markAsUnRead").parent().show();

                    $.post($("#urlMessageSetRead").val(), { 'msgId': msgId });
                    fnSetValue("#badgeNewMessages", parseInt($("#badgeNewMessages").text()) - 1);

                    row.addClass("isRead");
                });

            row
                .find("a.markAsUnRead")
                .click(function(e) {
                    e.preventDefault();
                    $(this).parent().hide();
                    $(this).parent().parent().find(".markAsRead").parent().show();


                    $.post($("#urlMessageSetUnread").val(), { 'msgId': msgId });
                    fnSetValue("#badgeNewMessages", parseInt($("#badgeNewMessages").text()) + 1);

                    row.removeClass("isRead");
                });
        });
    }
}