/* A shared class */
class QuestionsSearch extends SearchInTabs {

    constructor(fnOnLoad : () => void) {
        super(fnOnLoad);

        var filterSelector = "#txtCategoryFilter";
        var autoCompleteCategories = new AutocompleteCategories(filterSelector);
        autoCompleteCategories.OnAdd = (categoryId) => {
            this._categories.push(categoryId);
            this.SubmitSearch();
        };

        autoCompleteCategories.OnRemove = (categoryId) => {
            this._categories = $.grep(this._categories, (x) => { return x != categoryId; });
            this.SubmitSearch();
        };

        $(filterSelector).on("initCategoryIds", (e, categoryId) => {
            this._categories.push(categoryId);
        });

        if ($("#StatusFilterBar").length > 0) {
            $("#ckbFilterSolid, #ckbFilterConsolidation, #ckbFilterNeedsLearning, #ckbFilterNotLearned").change(() => {

                this._knowledgeFilter = new KnowledgeFilter();
                this._knowledgeFilter.Knowledge_Solid = $("#ckbFilterSolid").is(':checked');
                this._knowledgeFilter.Knowledge_ShouldConsolidate = $("#ckbFilterConsolidation").is(':checked');
                this._knowledgeFilter.Knowledge_ShouldLearn = $("#ckbFilterNeedsLearning").is(':checked');
                this._knowledgeFilter.Knowledge_None = $("#ckbFilterNotLearned").is(':checked');

                this.SubmitSearch();
            });
        }
    }
}

$(() => {
    new QuestionsSearch(() => {
        var page = new Page();
        page.Init();
    });
});