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

    var url = window.location.pathname;

    if (url.indexOf("/Lernen") >= 0)
        $("#LearningTab").click();
    else if (url.indexOf("/Analytics") >= 0)
        $("#AnalyticsTab").click();
    else
        $("#TopicTab").click();


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

    window.addEventListener('popstate', function (event) {

        if (url.indexOf("/Lernen") >= 0) {
            history.pushState(null, null, window.location.pathname);
            $("#LearningTab").click();
        }

        else if (url.indexOf("/Analytics") >= 0) {
            history.pushState(null, null, window.location.pathname);
            $("#AnalyticsTab").click();
        }
        else {
            history.pushState(null, null, window.location.pathname);
            $("#TopicTab").click();
        }
           
    }, false);


    var queryParams = Utils.GetQueryString();
    if (queryParams.openTab === "learningTab") {
        $("#LearningTab").click();
        
    }
});