var drawKnowledgeChart;

class SetPage {
    constructor() {

        this.InitPies();

        if ($("#hhdHasVideo").val() == "True") {
            new SetVideo();
        }

        new Pin(PinType.SetDetail, KnowledgeWheel.ReloadSet);
        new Pin(PinType.Question, KnowledgeWheel.ReloadSet);
    }

    InitPies() {
        $(".pieTotals").each(function () {
            var me = $(this);
            var values = $(this).attr("data-percentage").split('-');
            me.sparkline([values[0], values[1]], {
                type: 'pie',
                sliceColors: ['#90EE90', '#FFA07A']
            });
        });        
    }


}

$(() => {
    new SetPage();
});