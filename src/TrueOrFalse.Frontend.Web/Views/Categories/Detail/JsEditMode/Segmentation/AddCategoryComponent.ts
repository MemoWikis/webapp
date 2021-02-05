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
            existingCategoryName: "",
            existingCategoryUrl: "",
            showErrorMsg: false,
            disabled: false,
            addCategoryBtnId: null,
        };
    },
    watch: {
    },
    created() {
    },
    mounted() {
        $('#AddCategoryModal').on('show.bs.modal',
            event => {
                this.parentId = $('#AddCategoryModal').data('parent').id;
                this.addCategoryBtnId = $('#AddCategoryModal').data('parent').addCategoryBtnId;
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
            this.existingCategoryName = "";
            this.existingCategoryUrl = "";
        },
        closeModal() {
            $('#AddCategoryModal').modal('hide');
        },

        validateName() {
            var self = this;
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/ValidateName',
                data: JSON.stringify({ name: self.name }),
                success: function (data) {
                    if (data.categoryNameAllowed) {
                    } else {
                        self.errorMsg = data.errorMsg;
                        self.existingCategoryName = data.name;
                        self.existingCategoryUrl = data.url;
                        self.showErrorMsg = true;
                    };
                },
            });
        },
        addCategory() {
            var self = this;
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/ValidateName',
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
                                    window.open(data.url, "_blank");
                                    self.loadCategoryCard(data.id);
                                } else {
                                };
                            },
                        });
                    } else {
                        self.errorMsg = data.errorMsg;
                        self.existingCategoryName = data.name;
                        self.existingCategoryUrl = data.url;
                        self.showErrorMsg = true;
                    };
                },
            });
        },
        loadCategoryCard(id) {
            var self = this;

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryCard',
                data: JSON.stringify({ categoryId: id }),
                success: function (data) {
                    if (data) {
                        var inserted = $(data.html).insertBefore(self.addCategoryBtnId);
                        var instance = new categoryCardComponent({
                            el: inserted.get(0)
                        });
                    } else {

                    };
                },
            });
        },
    }
});