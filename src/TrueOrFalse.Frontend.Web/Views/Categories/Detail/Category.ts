class Tabbing {

    constructor() {

        $('#TabsBar .Tab').each(function () {

            var tab = $(this);
            tab.click(function(e) {
                e.preventDefault();
                if (!tab.hasClass('active')) {
                   Tabbing.ShowTab(tab.attr('id'));
                }
            });
        });
    }

    private static ShowTab(tabName: string): void {

        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + $("#hhdCategoryId").val();

        $.get(url, (html) => {

            $("#TabContent")
                .empty()
                .animate({ opacity: 0.00 }, 0)
                .append(html)
                .animate({ opacity: 1.00 }, 400);

            $("#TabContent .show-tooltip").tooltip();
        });

        $('.Tab').removeClass('active');
        $('#' + tabName).addClass('active');
    }

}

$(() => {
    new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
    new Pin(PinType.Set, KnowledgeBar.ReloadCategory);
    new Tabbing();
});