

var deleteCategoryComponent = Vue.component('delete-category-component', {
    data() {
        return {
            categoryName: '',
            categoryId: 0,
            showErrorMsg: false,
            errorMsg: '',
            hasChildren: false,
        };
    },
    watch: {
    },
    mounted() {
        eventBus.$on('open-delete-category-modal',
            id => {
                this.categoryId = id;
                this.loadCategoryData();
                $('#DeleteCategoryModal').modal('show');
            });
        $('#DeleteCategoryModal').on('show.bs.modal',
            event => {
            });

        $('#AddCategoryModal').on('hidden.bs.modal',
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
                    self.hasChildren = data.HasChildren;
                },
            });
        },
        deleteCategory() {
            $.ajax({
                type: 'POST',
                url: "/Categories/Delete/" + this.categoryId,
                cache: false,
                success: function () {
                    $('#forTheTimeToDeleteModal').modal('hide');
                    window.location.href = window.location.origin;
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