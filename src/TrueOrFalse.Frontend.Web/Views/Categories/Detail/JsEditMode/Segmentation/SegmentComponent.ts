Vue.component('segment-component', {
    props: {
        title: String,
        description: String,
        childCategoryIds: String,
        categoryId: [String, Number],
        editMode: Boolean,
        isMyWorld: Boolean,
        isHistoric: Boolean,
        parentId: Number
    },

    data() {
        return {
            categories: [],
            segmentId: null,
            cardsKey: null,
            isCustomSegment: true,
            selectedCategories: [],
            currentChildCategoryIds: [],
            currentChildCategoryIdsString: "",
            hover: false,
            showHover: false,
            addCategoryId: "",
            dropdownId: null,
            timer: null,
            linkToCategory: null,
            visibility: 0,
            segmentTitle: null,
            knowledgeBarHtml: null,
            disabled: true,
            knowledgeBarData: null
        };
    },
    mounted() {
        this.getSegmentData();
        this.segmentId = "Segment-" + this.categoryId;
        if (this.childCategoryIds != null) {
            var baseChildCategoryIds = JSON.parse(this.childCategoryIds);
            this.currentChildCategoryIds = baseChildCategoryIds;
        }
        this.addCategoryId = "AddCategoryTo-" + this.segmentId + "-Btn";
        this.dropdownId = this.segmentId + '-Dropdown';

        this.$on('select-category', (id) => this.selectCategory(id));
        this.$on('unselect-category', (id) => this.unselectCategory(id));
        eventBus.$on('add-category-card',
            (e) => {
                if (this.categoryId == e.parentId)
                    this.addNewCategoryCard(e.newCategoryId);
            });
        if (this.currentChildCategoryIds.length > 0)
            this.getCategoriesData();
    },

    watch: {
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        },
        currentChildCategoryIds() {
            this.currentChildCategoryIdsString = this.currentChildCategoryIds.join(',');
        },
        selectedCategoryIds(val) {
            this.disabled = val.length <= 0;
        }
    },

    updated() {
    },

    methods: {
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
                    self.$nextTick(() => Images.ReplaceDummyImages());
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
                },
            });
        },
        getSegmentData() {
            var self = this;
            var data = {
                categoryId: parseInt(self.categoryId),
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetSegmentData',
                data: JSON.stringify(data),
                success: function(data) {
                    self.linkToCategory = data.linkToCategory;
                    self.visibility = data.visibility;
                    self.knowledgeBarHtml = data.knowledgeBarHtml;
                    self.knowledgeBarData = data.knowledgeBarData;
                    if (self.title)
                        self.segmentTitle = self.title;
                    else self.segmentTitle = data.categoryName;
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
        updateCategoryOrder() {
            this.categories = $("#" + this.segmentId + " > .topic").map((idx, elem) => $(elem).attr("category-id")).get();
        },
        removeSegment() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveSegment");
                return;
            }
            var data = {
                parentId: this.parentId,
                newCategoryId: this.categoryId
            };
            eventBus.$emit('remove-segment', parseInt(this.categoryId));
            eventBus.$emit('add-category-card', data);
        },
        addCategory(val) {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            var self = this;
            var categoriesToFilter = Array.from(self.currentChildCategoryIds);
            categoriesToFilter.push(parseInt(self.categoryId));

            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#" + self.addCategoryId),
                moveCategories: false,
                categoriesToFilter,
                create: val
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
                    var removedChildCategoryIds = JSON.parse(result.removedChildCategoryIds);
                    self.filterChildren(removedChildCategoryIds);
                },
            });
        },
        filterChildren(selectedCategoryIds) {
            console.log(selectedCategoryIds);
            let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
                function (e) {
                    return this.indexOf(e) < 0;
                },
                selectedCategoryIds
            );
            this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
            this.selectedCategoryIds = [];
            eventBus.$emit('save-segments');
        },
        hideChildren() {
            this.filterChildren(this.selectedCategories);
        }
    },
});