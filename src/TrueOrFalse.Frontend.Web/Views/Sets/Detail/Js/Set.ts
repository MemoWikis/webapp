var drawKnowledgeChart;

class SetPage {
    constructor() {

        this.InitPies();

        if ($("#hhdHasVideo").val() == "True") {
            new SetVideo();
        }

        new Pin(PinType.SetDetail, this.ReloadKnowledgeChart);
        new Pin(PinType.Question, this.ReloadKnowledgeChart);
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

    ReloadKnowledgeChart() {

        $.get("/KnowledgeWheel/Get/?setId=" + $("#hhdSetId").val(), (html) => {

            $("#knowledgeWheelContainer")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            drawKnowledgeChart("chartKnowledge");
        });

    }
}

$(() => {
    new SetPage();
});