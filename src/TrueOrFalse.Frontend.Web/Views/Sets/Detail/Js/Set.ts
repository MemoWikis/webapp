$(function() {
    $(".pieTotals").each(function() {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#90EE90', '#FFA07A']
        });
    });

    new Pin(PinRowType.SetDetail, () => {
        var setId = $("#hhdSetId").val();
        $.post("/Set/GetRows", {id: setId}, (result) => {
            $("#rowContainer").fadeOut(250, () => {
                $("#rowContainer").html(result.Html);
                $("#rowContainer").fadeIn(250);
            });
        });
    });
    new Pin(PinRowType.Question);
});