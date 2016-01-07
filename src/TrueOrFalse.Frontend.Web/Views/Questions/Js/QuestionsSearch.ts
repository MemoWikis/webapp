/* A shared class */
class QuestionsSearch extends SearchInTabs {

    constructor(fnOnLoad : Function) {
        super(fnOnLoad);
        var filterSelector = "#txtCategoryFilter";
        var autoCompleteCategories = new AutocompleteCategories(filterSelector);
        autoCompleteCategories.OnAdd = (categoryId) => {
            this._categories.push(categoryId);
            this.SubmitSearch();
        };

        autoCompleteCategories.OnRemove = (categoryId) => {
            this._categories = $.grep(this._categories, (x) => { return x !== categoryId.toString(); });
            this.SubmitSearch();
        };

        $(filterSelector).on("initCategoryIds", (e, categoryId) => {
            this._categories.push(categoryId);
        });
    }
}

$(() => {
    new QuestionsSearch(() => {
        var page = new Page();
        page.Init();
    });
});