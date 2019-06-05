class CategoryPage {

    CategoryId;
    CategoryName;

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
        this.CategoryName = $("#hhdCategoryName").val();
    }

}

$(() => {
    var page = new CategoryPage();

    new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
    new Pin(PinType.Set, KnowledgeBar.ReloadCategory);
    new Tabbing(page);
    new CategoryHeader(page);
    new SquareWishKnowledge(page);

    $("#TopicTab").on("click",
        () => {
            window.history.pushState('Category', 'LearningTab', '/' + page.CategoryName + '/' + page.CategoryId);
        });

    $("#LearningTab").on("click",
        () => {
            window.history.pushState('Category', 'LearningTab', '/'+ page.CategoryName + '/' + page.CategoryId + '/Lernen');
        });

    $("#AnalyticsTab").on("click",
        () => {
            window.history.pushState('Category', 'LearningTab', '/' + page.CategoryName + '/' + page.CategoryId + '/Analytics');
        });
     
    var queryParams = Utils.GetQueryString();
    if (queryParams.openTab === "learningTab") {
        $("#LearningTab").click();
        
    }
});