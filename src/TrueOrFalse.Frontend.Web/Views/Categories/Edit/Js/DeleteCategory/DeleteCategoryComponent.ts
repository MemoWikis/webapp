

var deleteCategoryComponent = Vue.component('delete-category-component', {
    data() {
        return {
            categoryName: '',
            categoryId: 0,
            showErrorMsg: false,
            errorMsg: '',
        };
    },
    watch: {
    },
    mounted() {
        eventBus.$on('open-delete-category-modal',
            id => {
                this.categoryId = id;
                this.loadCategoryData();
            });
        $('#DeleteCategoryModal').on('show.bs.modal',
            event => {
            });

        $('#DeleteCategoryModal').on('hidden.bs.modal',
            event => {
            });
    },
    methods: {
        loadCategoryData() {
            var self = this;
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Categories/GetDeleteData',
                data: JSON.stringify({id: this.categoryId}),
                success: function (data) {
                    self.categoryName = data.CategoryName;
                    if (data.HasChildren) {
                        Alerts.showError({
                            text: messages.error.category.notLastChild
                        });
                    }

                    else
                        $('#DeleteCategoryModal').modal('show');
                },
            });
        },
        deleteCategory() {
            var self = this;
            $.ajax({
                type: 'POST',
                url: "/Categories/Delete/" + this.categoryId,
                cache: false,
                success: function () {
                    self.closeModal();
                    var allBreadcrumbLinks = $(".breadcrumb-item > span > a");
                    var hrefPathRedirect = "";
                    if (allBreadcrumbLinks.length > 1)
                        hrefPathRedirect = allBreadcrumbLinks[allBreadcrumbLinks.length - 2].href;
                    else
                        hrefPathRedirect = "/"; 
                    window.location.href = hrefPathRedirect; 
                },
                error: function (result) {
                    window.console.log(result);
                }
            });
        },
        clearData() {
        },
        closeModal() {
            $('#DeleteCategoryModal').modal('hide');
        },
    }
});