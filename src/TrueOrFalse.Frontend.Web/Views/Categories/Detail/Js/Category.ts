class CategoryPage {

    public CategoryId: number;
    private _categoryName: string;
    private _categoryVersion = null;
    private _history;
    private _arrayTabs;
    private Tab; 
    //doppelte Klicks auf ein Tab lösen ein zurückgehen aus.

    constructor() {
        this._history = [];
        this.CategoryId = $("#hhdCategoryId").val();
        this._categoryName = $("#hhdCategoryName").val();
        this.pushUrlAndSetactiveByClick(this._categoryName, this.CategoryId);
        this.Tab = new Tabbing(this.CategoryId);

        window.addEventListener('popstate',
            (event) => {
                console.log(event);
                
                var url = window.location.pathname;




                if (url.indexOf("Lernen") >= 0) {

                    if ($.trim($("#LearningTabContent").html()) === "")
                        this.Tab.RenderTabContent("LearningTab");
                    
                    if (!$("#LearningTabContent").is(':visible'))
                        $("#LearningTabContent").css("display", "block");

                    $("#TopicTabContent").css("display", "none");
                    $("#AnalyticsTabContent").css("display", "none");
                    
                }else if (url.indexOf("Analytics") >= 0) {

                        if ($.trim($("#AnalyticsTabContent").html()) === "")
                            this.Tab.RenderTabContent("AnalyticsTab");
                        
                        if (!$("#AnalyticsTabContent").is(':visible'))
                            $("#AnalyticsTabContent").css("display", "block");

                        $("#TopicTabContent").css("display", "none");
                        $("#LearningTabContent").css("display", "none");
                } 

                else {
                        if ($.trim($("#TopicTabContent").html()) === "")
                            this.Tab.RenderTabContent("TopicTab");
                   
                        if (!$("#TopicTabContent").is(':visible'))
                            $("#TopicTabContent").css("display", "block");

                        $("#LearningTabContent").css("display", "none");
                        $("#AnalyticsTabContent").css("display", "none");
                    
                }

  
            });
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
        window.history.pushState(this._history, 'Tab', currentUrl);
        this.hasAndSetTabActive();
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
   // new Tabbing(page.CategoryId);
    new CategoryHeader(page);
    new SquareWishKnowledge(page);
});