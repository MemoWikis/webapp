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
        isMyWorldString: String
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
    };
    },

    created() {
    },

    mounted() {
        if (this.childCategoryIds != null)
            this.currentChildCategoryIds = JSON.parse(this.childCategoryIds);
        if (this.segmentJson.length > 0)
            this.segments = JSON.parse(this.segmentJson);
        this.hasCustomSegment = this.segments.length > 0;

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
                    this.currentChildCategoryIds.push(e.newCategoryId);
            });

        eventBus.$on('category-data-is-loading', () => {
            this.loaded = false;
        });
        eventBus.$on('category-data-finished-loading', () => this.showComponents());
    },

    watch: {
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        }
    },

    updated() {
    },

    methods: {
        loadSegment(id) {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateSegment");
                return;
            }
            var idExists = (segment) => segment.CategoryId === id;
            if (this.segments.some(idExists))
                return;

            var self = this;
            var currentElement = $("#CustomSegmentSection");
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
                    } else {
                    };
                },
            });
        },
        addCategoryToBaseList(id) {
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryCard',
                data: JSON.stringify({ categoryId: id }),
                success: function (data) {
                    if (data) {
                        eventBus.$emit('save-segments');
                    } else {

                    };
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
        addCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            var self = this;
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                moveCategories: false,
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