/* A shared class */
class QuestionsSearch extends SearchInTabs {


    constructor() {
        super();
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


    }


}

$(function () {
    new QuestionsSearch();
})