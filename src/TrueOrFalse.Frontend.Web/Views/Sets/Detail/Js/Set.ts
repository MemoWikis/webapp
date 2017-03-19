var drawKnowledgeChart;

class SetPage {
    constructor() {

        this.InitPies();

        var setId = $("#hhdSetId").val();
        var setShare = new SetShare(setId);
        setShare.ShowModal(setId);

        if ($("#hhdHasVideo").val() == "True") {
            new SetVideo(() => { new Pin(PinType.Question, KnowledgeWheel.ReloadSet) });
        } else {
            new Pin(PinType.Question, KnowledgeWheel.ReloadSet);
        }

        new Pin(PinType.SetDetail, KnowledgeWheel.ReloadSet);
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