class _Dashboard {
    constructor() {
        $(".third-cell").on("click",
            ".fa-trash-o",
            () => {
              this.getCountDates($("#hddUserId").val());
            });

        if ($("#hddCountDates").val() === "0")
            $("#noOpenDates").fadeIn();

        $("#datesOverview").on("click",
            (e) => {
                e.preventDefault();
                $.ajax({
                    url: "/Dates/GetDatesOverview",
                    type: "POST",
                    success: (result) => {
                        console.log(result);
                        $(".content").html(result);
                    },
                    error: (e)=> {
                        console.log(e.statusText);
                    }
            });
            });
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

$(document).ready(() => {
    new _Dashboard();
});

   
