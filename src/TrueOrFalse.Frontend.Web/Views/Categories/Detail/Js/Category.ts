class CategoryPage {

    public CategoryId: number;
    private _categoryName: string;
    private _categoryVersion = null;
    private _tab: Tabbing;
    private _lastTabName: string;
    private _url: string; 

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
        this._categoryName = $("#hhdCategoryName").val();
        this.pushUrlAndSetactiveByClick(this._categoryName, this.CategoryId);
        this.hasAndSetTabActive();
        this._tab = new Tabbing(this.CategoryId);
        this._url = window.location.pathname;

        window.addEventListener('popstate',
            (event) => {
                this._url = window.location.pathname;

                this.hasAndSetTabActive();
                this.renderOrDisplayTab(this._url);
            });

        if (this._url.indexOf("Analytics") > 0 )
            this.renderOrDisplayTab(this._url);
    }

    private pushUrlAndSetactiveByClick(_categoryName: string, categoryId: number) {
        $("#TopicTab").on("click",
            () => {
                var url = '/' + this._categoryName + '/' + this.CategoryId;
                var tabname = "TopicTab";
                this.historyPushAndsetActive(url, tabname);
            });

        $("#LearningTab").on("click",
            () => {
                var url = '/' + this._categoryName + '/' + this.CategoryId + '/Lernen';
                var tabname = "LearningTab";
                this.historyPushAndsetActive(url, tabname);
            });

        $("#AnalyticsTab").on("click",
            (e) => {

                var url = '/' + this._categoryName + '/' + this.CategoryId + '/Analytics';
                var tabname = "Analyticstab";
                this.historyPushAndsetActive(url, tabname);
            });
    }

    private historyPushAndsetActive(currentUrl: string, tabname: string) {

        if (tabname !== this._lastTabName) {
            window.history.pushState(tabname, 'Tab', currentUrl);
            this.hasAndSetTabActive();
            this._lastTabName = tabname;
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

    private renderOrDisplayTab(url: string): void {
        if (url.indexOf("Lernen") >= 0) {

            if ($.trim($("#LearningTabContent").html()) === "") {
                this._tab.RenderTabContent("LearningTab");
                Utils.ShowSpinner();
            }
            if (!$("#LearningTabContent").is(':visible'))
                $("#LearningTabContent").css("display", "block");

            $("#TopicTabContent").css("display", "none");
            $("#AnalyticsTabContent").css("display", "none");

        } else if (url.indexOf("Analytics") >= 0) {

            if ($.trim($(".knowledgeGraphData").html()) === "") {
                this._tab.RenderTabContent("AnalyticsTab");
                Utils.ShowSpinner();
            }
            if (!$("#AnalyticsTabContent").is(':visible'))
                $("#AnalyticsTabContent").css("display", "block");

            $("#TopicTabContent").css("display", "none");
            $("#LearningTabContent").css("display", "none");
        }

        else {
            if ($.trim($("#TopicTabContent").html()) === "") {
                this._tab.RenderTabContent("TopicTab");
                Utils.ShowSpinner();
            }

            if (!$("#TopicTabContent").is(':visible'))
                $("#TopicTabContent").css("display", "block");

            $("#LearningTabContent").css("display", "none");
            $("#AnalyticsTabContent").css("display", "none");
        }
    }

}

$(() => {
    var page = new CategoryPage();

    new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
    new Pin(PinType.Set, KnowledgeBar.ReloadCategory);
    new CategoryHeader(page);
    new SquareWishKnowledge(page);
});