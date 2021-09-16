﻿Vue.component('category-card-component', {
    props: {
        categoryId: [String, Number],
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: [String, Number],
        hide: String,
        isMyWorld: Boolean,
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
                        let eventData = {
                            msg: messages.success.category[data.key]
                        }
                        eventBus.$emit('show-success', eventData);
                    }
                    else {
                        let eventData = {
                            msg: messages.error.category[data.key]
                        }
                        eventBus.$emit('show-error', eventData);
                    }
                },
            });
        },
        hideCategory() {
            this.$parent.filterChildren([this.categoryId]);
        },

    }
});
