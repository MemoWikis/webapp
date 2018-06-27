class _Dashboard {
    constructor() {
        $(".third-cell").on("click",
            ".fa-trash-o",
            () => {
              this.getCountDates($("#hddUserId").val());
            });

        if ($("#hddCountDates").val() === "0")
            $("#noOpenDates").fadeIn();
    }

    private getCountDates(userId: number) {
        $.ajax({
            'url': 'Knowledge/GetDatesCount',
            'type': 'POST',
            'data': {
                'userId': userId
    },
            'success': function (data) {
                debugger;
                if (data === "0") {
                    $("#noOpenDates").fadeIn();
                }
               
            },
            'error': function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }
}