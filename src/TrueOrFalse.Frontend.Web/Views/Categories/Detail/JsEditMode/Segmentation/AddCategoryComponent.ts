var addCategoryComponent = Vue.component('add-category-component', {
    props: {
        editMode: Boolean,
    },

    data() {
        return {
            name: "",
            private: false,
            errorMsg: "",
            parentId: null,
        };
    },
    watch: {
    },
    created() {
    },
    mounted() {
        $('#AddCategoryModal').on('show.bs.modal',
            event => {
                this.parentId = $('#AddCategoryModal').data('id');
            });

        $('#AddCategoryModal').on('hidden.bs.modal',
            event => {
                this.clearData();
            });
    },
    methods: {
        clearData() {
            this.name = "";
            this.private = false;
            this.errorMsg = false;
            this.parentId = null;
        },
        closeModal() {
            $('#AddCategoryModal').modal('hide');
        },

        addCategory() {
            var self = this;
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/NameCheck',
                data: JSON.stringify({ name: self.name }),
                success: function (data) {
                    if (data.categoryNameAllowed) {
                        var categoryData = {
                            name: self.name,
                            parentCategoryId: self.parentId
                        }
                        $.ajax({
                            type: 'Post',
                            contentType: "application/json",
                            url: '/EditCategory/QuickCreate',
                            data: JSON.stringify(categoryData),
                            success: function (data) {
                                if (data.success) {
                                    return true;
                                } else {

                                };
                            },
                        });
                    } else {
                        self.errorMsg = data.errorMsg;
                        return false;
                    };
                },
            });
        }
    }
});