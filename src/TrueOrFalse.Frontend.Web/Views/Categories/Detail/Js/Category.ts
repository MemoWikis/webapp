class CategoryPage {

    CategoryId;

    constructor() {
        this.CategoryId = $("#hhdCategoryId").val();
        if ($(".DescriptionSection > div:eq(1)").text() === "")
            $(".DescriptionSection").css("display", "none");
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
    if (queryParams.openTab === "learningTab")
        $("#LearningTab").click();
});