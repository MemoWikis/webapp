$(function () {
    new WidgetStats();
});

class WidgetStats {

    constructor() {
        var self = this;

        $(".selectWidgetForSingleView").change(function (e) {
            self.UpdateChart($(this).attr("data-host"), this.value, $(this).attr("data-target-div"));
        });
        
    }

    UpdateChart(host : string, widgetKey : string, targetDiv : string) {
        $.post("/Account/RenderWidgetStatsDetail/",
            { host: host, widgetKey: widgetKey },
            (result) => {
                $("#" + targetDiv).html(result);
            });

    }
}