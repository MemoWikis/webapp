class CategoryPage {

    public categoryId: number;
    private _categoryName: string;
    private _categoryVersion = null;
    private _history;
    private _arrayTabs;


    //Tabs werden noch nicht richtig aktiv geschaltet wenn reloadet wird 
    //doppelte Klicks auf ein Tab lösen ein zurückgehen aus.

    constructor() {
        this._history = [];
        this.categoryId = $("#hhdCategoryId").val();
        this._categoryName = $("#hhdCategoryName").val();
        var clicksToTab = 0;
        

        this.hasAndSetTabActive();

        $("#TopicTab").on("click",
            () => {
                var url = '/' + this._categoryName + '/' + this.categoryId;
                this.historyPush(url, "TopicTab");
                this.hasAndSetTabActive();
            });

        $("#LearningTab").on("click",
            () => {
                var url = '/' + this._categoryName + '/' + this.categoryId + '/Lernen';
                this.historyPush(url, "LearningTab");
                this.hasAndSetTabActive();
            });

        $("#AnalyticsTab").on("click",
            (e) => {
                var url = '/' + this._categoryName + '/' + this.categoryId + '/Analytics';
                this.historyPush(url, "Analyticstab");
                this.hasAndSetTabActive();
            });
    }

    private historyPush(currentUrl: string, tabname: string) {
        if (this._history.length === 0 || this._history[this._history.length - 1] !== currentUrl) {
            window.history.pushState(this._history, 'AnalyticsTab', currentUrl);
            this._history.push(currentUrl);
        } else if (this._history[this._history.length - 1] === currentUrl) {
            window.history.back();
        }
    }

    private hasAndSetTabActive() {
        var url = window.location.pathname; 
        if (url.indexOf("Lernen") >= 0 && !$("#LearningTab").hasClass("active")) {
            $("#LearningTab").addClass("active");
            $("#AnalyticsTab").removeClass("active");
            $("#TopicTab").removeClass("active");
        } else if (url.indexOf("Analytics") >= 0 && !$("#AnalyticsTab").hasClass("active")) {
            $("#LearningTab").removeClass("active");
            $("#AnalyticsTab").addClass("active");
            $("#TopicTab").removeClass("active");
        } else {
            $("#LearningTab").removeClass("active");
            $("#AnalyticsTab").removeClass("active");
            $("#TopicTab").addClass("active");
        }

    }
}

$(() => {
    var page = new CategoryPage();

    new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
    new Pin(PinType.Set, KnowledgeBar.ReloadCategory);
    new Tabbing(page);
    new CategoryHeader(page);
    new SquareWishKnowledge(page);
});