var addCategoryComponent = Vue.component('add-category-component', {
    data() {
        return {
            name: "",
            isPrivate: false,
            errorMsg: "",
            parentId: null,
            existingCategoryName: "",
            existingCategoryUrl: "",
            showErrorMsg: false,
            disabled: false,
            addCategoryBtnId: null,
            disableAddCategory: true,
            selectedCategories: [],
            moveCategories: false,
            parentIsPrivate: false,
        };
    },
    watch: {
        name(val) {
            if (val.length <= 0)
                this.disableAddCategory = true;
            else
                this.disableAddCategory = false;
        }
    },
    created() {
        var visibility = $('#hddVisibility').val();
        if (visibility != 'All') {
            this.parentIsPrivate = true;
            this.isPrivate = true;
        }
    },
    mounted() {
        $('#AddCategoryModal').on('show.bs.modal',
            event => {
                this.parentId = $('#AddCategoryModal').data('parent').id;
                this.addCategoryBtnId = $('#AddCategoryModal').data('parent').addCategoryBtnId;
                this.moveCategories = $('#AddCategoryModal').data('parent').moveCategories;
                if (this.moveCategories)
                    this.selectedCategories = $('#AddCategoryModal').data('parent').selectedCategories;
                if ($('#AddCategoryModal').data('parent').redirect != null &&
                    $('#AddCategoryModal').data('parent').redirect)
                    this.redirect = true;
            });

        $('#AddCategoryModal').on('hidden.bs.modal',
            event => {
                this.clearData();
            });
    },
    methods: {
        clearData() {
            this.name = "";
            this.isPrivate = false;
            this.errorMsg = "";
            this.showErrorMsg = false;
            this.parentId = null;
            this.existingCategoryName = "";
            this.existingCategoryUrl = "";
            this.selectedCategories = [];
            this.moveCategories = false;
            this.redirect = false;
        },
        closeModal() {
            $('#AddCategoryModal').modal('hide');
        },

        togglePrivacy() {
            if (this.parentIsPrivate)
                return;
            else
                this.isPrivate = !this.isPrivate;
        },

        addCategory() {
            Utils.ShowSpinner();
            var self = this;
            var url;
            var categoryData;
            if (this.moveCategories) {
                categoryData = {
                    name: self.name,
                    parentCategoryId: self.parentId,
                    isPrivate: self.isPrivate,
                    childCategoryIds: self.selectedCategories
                }
                url = '/EditCategory/QuickCreateWithCategories';

            } else {
                categoryData = {
                    name: self.name,
                    parentCategoryId: self.parentId,
                    isPrivate: self.isPrivate
                }
                url = '/EditCategory/QuickCreate';
            }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/ValidateName',
                data: JSON.stringify({ name: self.name }),
                success: function (data) {
                    if (data.categoryNameAllowed) {
                        $.ajax({
                            type: 'Post',
                            contentType: "application/json",
                            url: url,
                            data: JSON.stringify(categoryData),
                            success: function (data) {
                                if (data.success) {
                                    if (self.redirect)
                                        window.open(data.url, '_self');
                                    if (self.addCategoryBtnId != null)
                                        self.loadCategoryCard(data.id);
                                    if (self.moveCategories) {
                                        eventBus.$emit('remove-category-cards', data.movedCategories);
                                    }
                                    else
                                        $('#AddCategoryModal').modal('hide');
                                        Utils.HideSpinner();
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
            var data = {
                parentId: this.parentId,
                newCategoryId: id
            };
            eventBus.$emit('add-category-card', data);
        },
    }
});


var AddCategoryApp = new Vue({
    el: '#AddCategoryApp',
});