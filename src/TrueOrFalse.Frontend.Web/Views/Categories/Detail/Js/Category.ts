﻿class CategoryPage {

    public CategoryId: number;
    private _categoryName: string;
    private _categoryVersion = null;
    private _tab: Tabbing;
    private _lastTabName: string;
    private _url: string;

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
        this._categoryName = $("#hhdCategoryName").val();
        this.pushUrlAndSetActiveByClick(this._categoryName);
        this.hasAndSetTabActive();
        this._tab = new Tabbing(this.CategoryId);
        this._url = window.location.pathname;

        window.addEventListener('popstate',
            (event) => {
                this._url = window.location.pathname;

                this.hasAndSetTabActive();
                this.renderOrDisplayTab(this._url);
            });

        this.renderOrDisplayTab(this._url);

        let jumpToSegmentationBtn = document.getElementById("JumpToSegmentationBtn");
        jumpToSegmentationBtn.addEventListener("click", (e: Event) => this.jumpToSegmentation());
    }

    private jumpToSegmentation() {
        if (document.getElementById('LearningTabWithOptions').classList.contains('active')) {
            document.getElementById('TopicTab').click();
            setTimeout(() => {
                    document.getElementById('SegmentationTop')
                        .scrollIntoView({ behavior: "smooth", block: "start", inline: "start" });
                },
                500);
        } else {
            document.getElementById('SegmentationTop')
                .scrollIntoView({ behavior: "smooth", block: "start", inline: "start" });
        }
    }

    private pushUrlAndSetActiveByClick(_categoryName: string) {

            $("#TopicTab").on("click",
                (e) => {
                    e.preventDefault();
                    this.historyPushAndSetActive($("#TopicTab").attr("data-url"), "TopicTab");
                });

            $("#LearningTab,#LearningFooterBtn").on("click",
                (e) => {
                    e.preventDefault();
                    this.historyPushAndSetActive($("#LearningTab").attr("data-url"), "LearningTab");
                });

            $("#AnalyticsTab,#AnalyticsFooterBtn").on("click",
                (e) => {
                    e.preventDefault();
                    this.historyPushAndSetActive($("#AnalyticsTab").attr("data-url"), "AnalyticsTab");
                });
    }


    private historyPushAndSetActive(currentUrl: string, tabName: string) {

        if (tabName != this._lastTabName) {
            window.history.pushState(tabName, 'Tab', currentUrl);
            this.hasAndSetTabActive();
            this._lastTabName = tabName;
        }
    }

    private hasAndSetTabActive() {
        let url = window.location.pathname;
        if (url.indexOf("Lernen") > 0 && !$("#LearningTabWithOptions").hasClass("active")) {
            $("#AnalyticsTab").removeClass("active");
            $("#TopicTab").removeClass("active");
            $("#LearningTabWithOptions").addClass("active");
        } else if (url.indexOf("Wissensnetz") > 0 && !$("#AnalyticsTab").hasClass("active")) {
            $("#LearningTabWithOptions").removeClass("active");
            $("#TopicTab").removeClass("active");
            $("#AnalyticsTab").addClass("active");
        } else {
            $("#LearningTabWithOptions").removeClass("active");
            $("#AnalyticsTab").removeClass("active");
            $("#TopicTab").addClass("active");
        }
    }

    private renderOrDisplayTab(url: string): void {
        if (url.indexOf("Lernen") > 0) {

            if ($.trim($("#LearningTabContent").html()) == "") {
                this._tab.RenderTabContent("LearningTab");
                Utils.ShowSpinner();
            }
            if (!$("#LearningTabContent").is(':visible'))
                $("#LearningTabContent").css("display", "block");

            $("#TopicTabContent").css("display", "none");
            $("#AnalyticsTabContent").css("display", "none");

        } else if (url.indexOf("Wissensnetz") > 0) {

            if ($.trim($(".knowledgeGraphData").html()) == "") {
                this._tab.RenderTabContent("AnalyticsTab");
                Utils.ShowSpinner();
            }
            if (!$("#AnalyticsTabContent").is(':visible'))
                $("#AnalyticsTabContent").css("display", "block");

            $("#TopicTabContent").css("display", "none");
            $("#LearningTabContent").css("display", "none");
        } else {
            if ($.trim($("#TopicTabContent").html()) == "") {
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
    new SquareWishKnowledge(page);
});