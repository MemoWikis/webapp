new Vue({
    el: '#CategoryHeader',
    data() {
        return {
            showMyWorld: false,
            editCategoryName: false,
            categoryName: "",
        }
    },
    created() {
        this.categoryName = $('#HeadingContainer').data('category-name');
    }
})