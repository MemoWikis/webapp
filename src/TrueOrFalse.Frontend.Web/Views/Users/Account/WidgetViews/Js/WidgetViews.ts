$(function () {
    new WidgetView();
});

class WidgetView {

    constructor() {
        var self = this;

        $(".selectWidgetForSingleView").change(function (e) {
            self.UpdateChart($(this).attr("data-host"), this.value);
        });
        
    }

    UpdateChart(host : string, widgetKey : string) {

        console.log("Host: " + host + " -- Key: " + widgetKey);

    }
}