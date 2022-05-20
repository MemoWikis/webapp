Vue.component('category-card-component', {
    props: {
        categoryId: [String, Number],
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: [String, Number],
        hide: String,
        category: Object,
        isHistoric: Boolean,
    },

    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null,
            id: parseInt(this.categoryId),
            isSelected: false,
            checkboxId: '',
            showHover: false,
            imgHtml: null,
            linkToCategory: null,
            visibility: 0,
            categoryTypeHtml: null,
            childCategoryCount: 0,
            questionCount: 0,
            categoryName: null,
            knowledgeBarHtml: null,
        };
    },

    mounted() {
        this.$nextTick(() => {
            Images.ReplaceDummyImages();
        });
        eventBus.$on('publish-category',
            id => {
                if (this.id == id)
                    this.visibility = 0;
            });
        $('.show-tooltip').tooltip();
    },
    watch: {
        selectedCategories() {
            this.isSelected = this.selectedCategories.includes(this.id);
        },
        hover(val) {
            this.showHover = val;
        },
        categoryId() {
            this.init();
        }
    },
    created() {
        this.init();
    },
    methods: {
        mouseOver(event) {
            event.stopPropagation();
            this.hover = true;
        },
        mouseLeave(event) {
            event.stopPropagation();
            this.hover = false;
        },
        goToCategory() {
            window.location.href = this.category.LinkToCategory;
        },
        init() {
            this.dropdownId = this.segmentId + '-Dropdown' + this.id;
            this.checkboxId = this.segmentId + '-Checkbox' + this.id;
            if (this.isCustomSegment)
                this.dropdownId += this.$parent.id;

            this.visibility = this.category.Visibility;
        },

        thisToSegment() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("MoveToSegment");
                return;
            }
            if (!this.isCustomSegment) {
                this.$parent.loadSegment(this.id);
            }
        },
        removeParent() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveParent");
                return;
            }
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveParent',
                data: JSON.stringify(data),
                success: function (data) {
                    if (data.success == true) {
                        self.$parent.currentChildCategoryIds = self.$parent.currentChildCategoryIds.filter((id) => {
                            return id != self.categoryId;
                        });
                        self.$parent.categories = self.$parent.categories.filter((c) => {
                            return c.Id != self.categoryId;
                        });

                        Alerts.showSuccess({
                            text: messages.success.category[data.key]
                        });
                    }
                    else {
                        Alerts.showError({
                            text: messages.error.category[data.key]
                        });
                    }
                },
            });
        },
        openMoveCategoryModal() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("MoveCategory");
                return;
            }

            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            eventBus.$emit('open-move-category-modal', data);
        },
        hideCategory() {
            this.$parent.filterChildren([this.categoryId]);
        },
        openPublishModal() {
            eventBus.$emit('open-publish-category-modal', this.categoryId);
        }

    }
});
