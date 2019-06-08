class CategoryPage {

    CategoryId: number;
    CategoryName: string;
    Categoryversion = null;
    Url: string;
    isPushed: boolean = false;

    constructor() {
        var myHistory = [];
        this.CategoryId = $("#hhdCategoryId").val();
        this.CategoryName = $("#hhdCategoryName").val();

        $("#TopicTab").on("click",
            () => {
                var url =
                    '/' + this.CategoryName + '/' + this.CategoryId;
                if (myHistory.length === 0 || myHistory[myHistory.length - 1] !== url) {
                    window.history.pushState(myHistory,'TopicTab', url);
                    myHistory.push(url);
                } else if (myHistory[myHistory.length - 1] === url) {
                    window.history.back();
                }
            });

        $("#LearningTab").on("click",
            () => {
                var url = '/' + this.CategoryName + '/' + this.CategoryId + '/Lernen';
                if (myHistory.length === 0 || myHistory[myHistory.length - 1] !== url ) {
                    window.history.pushState('Category',
                        'LearningTab', url);
                    myHistory.push(url);
                } else if (myHistory[myHistory.length - 1] === url) {
                    window.history.back();
                } 
            });

        $("#AnalyticsTab").on("click",
            (e) => {
                var url = '/' + this.CategoryName + '/' + this.CategoryId + '/Analytics';
                if (myHistory.length === 0 || myHistory[myHistory.length - 1] !== url) {
                    window.history.pushState(myHistory,
                        'AnalyticsTab', url);
                    myHistory.push(url);
                }
                else if (myHistory[myHistory.length - 1] === url) {
                    window.history.back();
                }
            });

        window.addEventListener('popstate', (e) => {
            this.Url = window.location.pathname;

            if (this.Url.indexOf("Lernen") >= 0) {
                $("#LearningTab").click();
            }
            else if (this.Url.indexOf("Analytics") >= 0) {
                $("#AnalyticsTab").click();
            }
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
});