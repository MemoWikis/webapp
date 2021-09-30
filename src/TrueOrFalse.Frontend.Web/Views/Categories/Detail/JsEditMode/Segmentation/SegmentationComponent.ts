interface Segment {
    CategoryId: Number,
    Title: String,
    ChildCategoryIds: Array<Number>,
};


var segmentationComponent = Vue.component('segmentation-component', {
    props: {
        categoryId: [String, Number],
        editMode: Boolean,
        childCategoryIds: String,
        segmentJson: String,
        isMyWorldString: String,
        isHistoricString: String,
    },

    data() {
        return {
            baseCategoryList: [],
            componentKey: 0,
            selectedCategoryId: null,
            isCustomSegment: false,
            hasCustomSegment: false,
            selectedCategories: [],
            segmentId: 'SegmentationComponent',
            hover: false,
            showHover: false,
            addCategoryId: "AddToCurrentCategoryCard",
            dropdownId: "MainSegment-Dropdown",
            controlWishknowledge: false,
            loadComponents: true,
            currentChildCategoryIds: [],
            segments: [] as Segment[],
            isMyWorld: this.isMyWorldString == 'True',
            categories: [],
            isHistoric: this.isHistoricString == 'True',
        };
    },
    mounted() {
        if (this.childCategoryIds != null)
            this.currentChildCategoryIds = JSON.parse(this.childCategoryIds);
        if (this.segmentJson.length > 0)
            this.segments = JSON.parse(this.segmentJson);
        this.hasCustomSegment = this.segments.length > 0;
        if (this.currentChildCategoryIds.length > 0)
            this.getCategoriesData();
        var self = this;
        eventBus.$on('remove-segment', (id) => {
            this.segments = this.segments.filter(s => s.CategoryId != id);
            this.currentChildCategoryIds.push(id);
            this.hasCustomSegment = this.segments.length > 0;
            eventBus.$emit('save-segments');
        });
        eventBus.$on('remove-category-cards',
            ids => {
                ids.map(id => {
                    self.$refs['card' + id].visible = false;
                });
            });
        eventBus.$on('add-category-card',
            (e) => {
                if (e.parentId == this.categoryId)
                    this.addNewCategoryCard(e.newCategoryId);
            });

        eventBus.$on('category-data-is-loading', () => {
            this.loaded = false;
        });
        eventBus.$on('category-data-finished-loading', () => this.showComponents());
        eventBus.$on('add-category',
            () => {
                this.$nextTick(() => {
                    var categoriesToFilter = this.setCategoriesToFilter();
                    eventBus.$emit('set-categories-to-filter', categoriesToFilter);
                });
            });

    },

    watch: {
        hover(val) {
            this.showHover = !!(val && this.editMode);
        },
        currentChildCategoryIds() {
            var categoriesToFilter = this.setCategoriesToFilter();
            eventBus.$emit('set-categories-to-filter', categoriesToFilter);
        },
        segments() {
            var categoriesToFilter = this.setCategoriesToFilter();
            eventBus.$emit('set-categories-to-filter', categoriesToFilter);
        }
    },
    methods: {
        setCategoriesToFilter() {
            var categoriesToFilter = Array.from(this.currentChildCategoryIds);
            categoriesToFilter.push(parseInt(this.categoryId));
            this.segments.forEach(s => {
                categoriesToFilter.push(s.CategoryId);
            });

            return categoriesToFilter;
        },
        addNewCategoryCard(id) {
            var self = this;
            var data = {
                categoryId: id,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryData',
                data: JSON.stringify(data),
                success: function (c) {
                    self.categories.push(c);
                    self.currentChildCategoryIds.push(c.Id);

                },
            });
        },
        getCategoriesData() {
            var self = this;
            var data = {
                categoryIds: self.currentChildCategoryIds,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoriesData',
                data: JSON.stringify(data),
                success: function (data) {
                    data.forEach(c => self.categories.push(c));
                    self.$nextTick(() => Images.ReplaceDummyImages());
                },
            });
        },
        getCategory(id) {
            var self = this;
            var data = {
                id: id,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryData',
                data: JSON.stringify(data),
                success: function (c) {
                    self.categories.push(c);
                    self.$nextTick(() => Images.ReplaceDummyImages());
                },
            });
        },
        loadSegment(id) {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateSegment");
                return;
            }
            var idExists = (segment) => segment.CategoryId === id;
            if (this.segments.some(idExists))
                return;

            this.currentChildCategoryIds = this.currentChildCategoryIds.filter(c => c != id);
            this.categories = this.categories.filter(c => c.Id != id);

            var self = this;
            var data = { CategoryId: id }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetSegment',
                data: JSON.stringify(data),
                success: function(segment) {
                    if (segment) {
                        self.hasCustomSegment = true;
                        var index = self.segments.indexOf(segment);
                        if (index == -1)
                            self.segments.push(segment);
                        eventBus.$emit('save-segments');
                    }
                },
            });
        },
        selectCategory(id) {
            if (this.selectedCategories.includes(id))
                return;
            else this.selectedCategories.push(id);
        },
        unselectCategory(id) {
            if (this.selectedCategories.includes(id)) {
                var index = this.selectedCategories.indexOf(id);
                this.selectedCategories.splice(index, 1);
            }
        },
        addCategory(val) {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }

            var self = this;
            var categoriesToFilter = this.setCategoriesToFilter();
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                moveCategories: false,
                categoriesToFilter,
                create: val,
            }
            $('#AddCategoryModal').data('parent', parent).modal('show');
        },
        removeChildren() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveChildren");
                return;
            }
            var self = this;
            var data = {
                parentCategoryId: self.categoryId,
                childCategoryIds: self.selectedCategories,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveChildren',
                data: JSON.stringify(data),
                success: function (result) {
                    eventBus.$emit('content-change');
                    var removedChildCategoryIds = JSON.parse(result.removedChildCategoryIds);
                    self.filterChildren(removedChildCategoryIds);
                },
            });
        },
        moveToNewCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("MoveToNewCategory");
                return;
            }
            var self = this;
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                moveCategories: true,
                selectedCategories: self.selectedCategories,
            }
            $('#AddCategoryModal').data('parent', parent).modal('show');
        },
        showComponents: _.debounce(function() {
            this.loaded = true;
        }, 1000),
        filterChildren(selectedCategoryIds) {
            let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
                function (e) {
                    return this.indexOf(e) < 0;
                },
                selectedCategoryIds
            );
            this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
            eventBus.$emit('save-segments');
        }
    },
});