class CategoryPage {

    CategoryId;
    CategoryName;

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
        this.CategoryName = $("#hhdCategoryName").val();

        var url = window.location.pathname;

        $("#TopicTab").on("click",
            () => {
                window.history.pushState('Category', 'LearningTab', '/' + this.CategoryName + '/' + this.CategoryId + "#");
            });

        $("#LearningTab").on("click",
            () => {
                window.history.pushState('Category', 'LearningTab', '/' + this.CategoryName + '/' + this.CategoryId + '/Lernen#');
            });

        $("#AnalyticsTab").on("click",
            () => {
                window.history.pushState('Category', 'LearningTab', '/' + this.CategoryName + '/' + this.CategoryId + '/Analytics#');
            });

        window.addEventListener('popstate', function (event) {


        }, false);
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