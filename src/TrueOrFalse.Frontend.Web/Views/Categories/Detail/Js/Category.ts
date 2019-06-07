class CategoryPage {

    CategoryId;
    CategoryName;
    Categoryversion = null;
    Url;

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
        this.CategoryName = $("#hhdCategoryName").val();

        $("#TopicTab").on("click",
            () => {
                window.history.pushState('Category',
                    'LearningTab',
                    '/' + this.CategoryName + '/' + this.CategoryId + "#");
            });

        $("#LearningTab").on("click",
            () => {
                window.history.pushState('Category',
                    'LearningTab',
                    '/' + this.CategoryName + '/' + this.CategoryId + '/Lernen');
            });

        $("#AnalyticsTab").on("click",
            () => {
                window.history.pushState('Category',
                    'LearningTab',
                    '/' + this.CategoryName + '/' + this.CategoryId + '/Analytics#');
            });

        window.addEventListener('popstate', (e) => {
            this.Url = window.location.pathname;
            if (this.Url.indexOf("Lernen") >= 0)
                $("#LearningTab").click();
            else if (this.Url.indexOf("Analytics") >= 0)
                $("#AnalyticsTab").click();
            else {
                $("#TopicTab").click();
            }
        });


    }
}

$(() => {
    var page = new CategoryPage();

    new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
    new Pin(PinType.Set, KnowledgeBar.ReloadCategory);
    new Tabbing(page);
    new CategoryHeader(page);
    new SquareWishKnowledge(page);

    var queryParams = Utils.GetQueryString();
    //if (queryParams.openTab === "learningTab") {
    //    $("#LearningTab").click();
        
    //}
   


});