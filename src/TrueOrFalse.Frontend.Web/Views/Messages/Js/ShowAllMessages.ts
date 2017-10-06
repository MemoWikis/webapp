class ShowAllMessages {

    constructor() {
        $("#btnShowAllMessages").click((e) => {
            e.preventDefault();
            this.ShowAllMessagesRendered();
        });
    }

    ShowAllMessagesRendered() {
        $.ajax({
            type: 'POST',
            url: "/Messages/RenderAllMessagesInclRead",
            cache: false,
            success: function (result) {
                $("#messagesWrapper").html(result);
            },
            error: function (e) {
                console.log(e);
                window.alert("Ein Fehler ist aufgetreten.");
            }
        });

    }
}