const EditCategoryRelationType = {
    Create: "Create",
    Move: "Move",
    AddParent: "AddParent",
    AddChild: "AddChild",
    None: "None",
    AddToWiki: "AddToWiki"
};

var addCategoryComponent = Vue.component('add-category-component', {

    data() {
        return {
            name: "",
            errorMsg: "",
            parentId: null,
            parentCategoryIdToRemove: null,
            childId: null,
            forbiddenCategoryName: "",
            existingCategoryUrl: "",
            showErrorMsg: false,
            disabled: false,
            addCategoryBtnId: null,
            disableAddCategory: true,
            selectedCategories: [],
            editCategoryRelation: EditCategoryRelationType.None,
            editCategoryRelationType: EditCategoryRelationType,
            categories: [],
            searchTerm: "",
            totalCount: 0,
            selectedCategoryId: 0,
            debounceSearchCategory: _.debounce(this.searchCategory, 300),
            showDropdown: false,
            lockDropdown: true,
            selectedCategory: null,
            showSelectedCategory: false,
            categoriesToFilter: [],
            personalWiki: null,
            addToWikiHistory: null,
            hideSearch: true,
            selectedParentInWikiId: null,
        };
    },
    watch: {
        name(val) {
            if (val.length <= 0)
                this.disableAddCategory = true;
            else
                this.disableAddCategory = false;
        },
        searchTerm(term) {
            if (term.length > 0 && this.lockDropdown == false) {
                this.showDropdown = true;
                this.debounceSearchCategory();
            }
            else
                this.showDropdown = false;
        },
        selectedCategoryId(id) {
            if (id > 0 && !this.createCategory)
                this.disableAddCategory = false;
        },
        createCategory() {
            if (this.selectedCategory != null)
                this.showSelectedCategory = true;
        }
    },
    mounted() {
        eventBus.$on('set-categories-to-filter', (ids) => {
            this.$nextTick(() => {
                this.categoriesToFilter = ids;
            });
        });
        eventBus.$on('create-category',
            id => {
                var parent = {
                    id: id,
                    addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                    redirect: true,
                    editCategoryRelation: EditCategoryRelationType.Create
                }
                if ($('#LearningTabWithOptions').hasClass("active"))
                    parent.addCategoryBtnId = null;
                $('#AddCategoryModal').data('parent', parent).modal('show');
            });
        eventBus.$on('add-parent-category',
            id => {
                var parent = {
                    id: id,
                    addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                    redirect: true,
                    editCategoryRelation: EditCategoryRelationType.AddParent
                }
                this.childId = id;
                if ($('#LearningTabWithOptions').hasClass("active"))
                    parent.addCategoryBtnId = null;
                $('#AddCategoryModal').data('parent', parent).modal('show');
            });
        eventBus.$on('add-child-category',
            id => {
                var parent = {
                    id: id,
                    addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                    redirect: true,
                    editCategoryRelation: EditCategoryRelationType.AddChild
                }
                if ($('#LearningTabWithOptions').hasClass("active"))
                    parent.addCategoryBtnId = null;
                $('#AddCategoryModal').data('parent', parent).modal('show');
            });
        eventBus.$on('add-to-personal-wiki',
            id => {
                var data = {
                    categoryId: id
                };
                $.ajax({
                    type: 'Post',
                    contentType: "application/json",
                    url: '/EditCategory/AddToPersonalWiki',
                    data: JSON.stringify(data),
                    success: function (data) {
                        if (data.success) {
                            Alerts.showSuccess({
                                text: messages.success.category[data.key]
                            });
                            Utils.HideSpinner();
                        } else {
                            Alerts.showError({
                                text: messages.error.category[data.key]
                            });
                            Utils.HideSpinner();
                        };
                    },
                });
            });
        eventBus.$on('add-to-wiki',
            id => {
                var parent = {
                    id: id,
                    addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                    redirect: true,
                    editCategoryRelation: EditCategoryRelationType.AddToWiki
                }
                this.childId = id;
                if ($('#LearningTabWithOptions').hasClass("active"))
                    parent.addCategoryBtnId = null;
                $('#AddCategoryModal').data('parent', parent).modal('show');
            });
        eventBus.$on('open-move-category-modal', this.fillMoveModal);

        $('#AddCategoryModal').on('show.bs.modal',
            event => {
                this.editCategoryRelation = $('#AddCategoryModal').data('parent').editCategoryRelation;
                this.parentId = $('#AddCategoryModal').data('parent').id;
                this.addCategoryBtnId = $('#AddCategoryModal').data('parent').addCategoryBtnId;
                if ($('#AddCategoryModal').data('parent').redirect != null &&
                    $('#AddCategoryModal').data('parent').redirect)
                    this.redirect = true;

                if (this.editCategoryRelation == EditCategoryRelationType.AddToWiki)
                    this.initWikiData();

                this.categoriesToFilter = $('#AddCategoryModal').data('parent').categoriesToFilter;
            });

        $('#AddCategoryModal').on('hidden.bs.modal',
            event => {
                Object.assign(this.$data, this.$options.data.apply(this));
            });
    },
    methods: {
        closeModal() {
            $('#AddCategoryModal').modal('hide');
        },

        fillMoveModal(data) {
            this.parentCategoryIdToRemove = data.parentCategoryIdToRemove;
            this.childId = data.childCategoryId;

            var parent = {
                id: this.childId,
                addCategoryBtnId: null,
                redirect: true,
                editCategoryRelation: EditCategoryRelationType.Move
            }

            $('#AddCategoryModal').data('parent', parent).modal('show');
        },

        addCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            Utils.ShowSpinner();
            var self = this;
            var url;
            var categoryData;

            categoryData = {
                name: self.name,
                parentCategoryId: self.parentId,
            }
            url = '/EditCategory/QuickCreate';
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/ValidateName',
                data: JSON.stringify({ name: self.name }),
                success(data) {
                    if (data.categoryNameAllowed) {
                        $.ajax({
                            type: 'Post',
                            contentType: "application/json",
                            url: url,
                            data: JSON.stringify(categoryData),
                            success(data) {

                                if (data.success) {
                                    if (self.redirect)
                                        window.open(data.url, '_self');

                                    if (self.addCategoryBtnId != null)
                                        self.loadCategoryCard(data.id);

                                    $('#AddCategoryModal').modal('hide');
                                    self.addCategoryCount();
                                    Utils.HideSpinner();
                                }
                            },
                        });
                    } else {
                        self.errorMsg = messages.error.category[data.key];
                        self.forbiddenCategoryName = data.name;
                        self.existingCategoryUrl = data.url;
                        self.showErrorMsg = true;
                        Utils.HideSpinner();
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
        selectCategory(category) {
            this.showDropdown = false;
            this.lockDropdown = true;
            this.searchTerm = category.Name;
            this.selectedCategory = category;
            this.selectedCategoryId = category.Id;
            this.showSelectedCategory = true;

            this.selectedParentInWikiId = category.Id;
        },
        toggleShowSelectedCategory() {
            this.showSelectedCategory = false;
            this.$nextTick(() => {
                this.$refs.searchInput.focus();
            });
        },
        searchCategory() {
            this.showDropdown = true;
            var self = this;
            var data = {
                term: self.searchTerm,
                type: 'Categories',
                categoriesToFilter: self.categoriesToFilter,
            };
            var url = this.editCategoryRelation == this.editCategoryRelationType.AddToWiki
                ? '/Api/Search/CategoryInWiki'
                : '/Api/Search/Category';
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: url,
                data: JSON.stringify(data),
                success(result) {
                    self.categories = result.categories.filter(c => c.Id != self.parentId);
                    self.totalCount = result.totalCount;
                    self.$nextTick(() => {
                        $('[data-toggle="tooltip"]').tooltip();
                    });
                },
            });
        },
        addExistingCategory() {
            Utils.ShowSpinner();
            var self = this;
            var categoryData = {
                childCategoryId: self.selectedCategoryId,
                parentCategoryId: self.parentId,
            }

            if (this.selectedCategoryId == this.parentId) {
                this.errorMsg = messages.error.category.loopLink;
                this.showErrorMsg = true;
                Utils.HideSpinner();
                return;
            }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/AddChild',
                data: JSON.stringify(categoryData),
                success(data) {
                    if (data.success) {
                        if (self.redirect)
                            window.open(data.url, '_self');

                        if (self.addCategoryBtnId != null)
                            self.loadCategoryCard(data.id);

                        $('#AddCategoryModal').modal('hide');
                        self.addCategoryCount();
                        Utils.HideSpinner();
                    } else {
                        self.errorMsg = messages.error.category[data.key];
                        self.showErrorMsg = true;
                        Utils.HideSpinner();
                    };
                },
            });
        },
        moveCategoryToNewParent() {
            Utils.ShowSpinner();
            var self = this;
            var categoryData = {
                childCategoryId: self.childId,
                parentCategoryIdToRemove: self.parentCategoryIdToRemove,
                parentCategoryIdToAdd: this.selectedCategory.Id
            }

            if (this.selectedCategoryId == this.parentId) {
                this.errorMsg = messages.error.category.loopLink;
                this.showErrorMsg = true;
                Utils.HideSpinner();
                return;
            }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/MoveChild',
                data: JSON.stringify(categoryData),
                success(data) {
                    if (data.success) {
                        if (self.redirect)
                            window.open(data.url, '_self');
                        if (self.addCategoryBtnId != null)
                            self.loadCategoryCard(data.id);
                        else
                            $('#AddCategoryModal').modal('hide');
                        self.addCategoryCount();
                        Utils.HideSpinner();
                    } else {
                        self.errorMsg = messages.error.category[data.key];
                        self.showErrorMsg = true;
                        Utils.HideSpinner();
                    };
                },
            });
        },
        addNewParentToCategory() {
            Utils.ShowSpinner();
            var self = this;
            var isAddToWiki = self.editCategoryRelation == EditCategoryRelationType.AddToWiki;
            var parentCategoryId = isAddToWiki ? self.selectedParentInWikiId : self.selectedCategory.Id;
            var categoryData = {
                childCategoryId: self.childId,
                parentCategoryId: parentCategoryId,
                redirectToParent: true,
                addIdToWikiHistory: isAddToWiki
            }

            if (this.selectedCategoryId == this.parentId) {
                this.errorMsg = messages.error.category.loopLink;
                this.showErrorMsg = true;
                Utils.HideSpinner();
                return;
            }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/AddChild',
                data: JSON.stringify(categoryData),
                success(data) {
                    if (data.success) {
                        if (self.redirect)
                            window.open(data.url, '_self');
                        if (self.addCategoryBtnId != null)
                            self.loadCategoryCard(data.id);
                        else
                            $('#AddCategoryModal').modal('hide');
                        self.addCategoryCount();
                        Utils.HideSpinner();
                    } else {
                        self.errorMsg = messages.error.category[data.key];
                        self.showErrorMsg = true;
                        Utils.HideSpinner();
                    };
                },
            });
        },
        addCategoryCount() {
            let headerCount = parseInt($('#CategoryHeaderTopicCount').text());
            $('#CategoryHeaderTopicCount').text(++headerCount);
            headerCount != 1
                ? $('#CategoryHeaderTopicCountLabel').text('Unterthemen')
                : $('#CategoryHeaderTopicCountLabel').text('Unterthema');
        },
        initWikiData() {
            var self = this;

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                data: JSON.stringify({ id: self.parentId }),
                url: '/Api/Search/GetPersonalWikiData',
                success(result) {
                    if (result.success) {
                        self.personalWiki = result.personalWiki;
                        self.addToWikiHistory = result.addToWikiHistory.reverse();
                        self.categoriesToFilter = [];
                        self.categoriesToFilter.push(self.personalWiki.Id);
                        self.addToWikiHistory.forEach((el) => {
                            self.categoriesToFilter.push(el.Id);
                        });

                        self.selectedParentInWikiId = self.personalWiki.Id;
                        self.disableAddCategory = false;
                    }
                },
            });
        }
    }
});

var AddCategoryApp = new Vue({
    name: 'AddCategory',
    el: '#AddCategoryApp',
});