var categoryCardComponent = Vue.component('category-card-component', {
    props: {
        categoryId: [String, Number],
        editMode: Boolean,
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: String,
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
        };
    },
    watch: {
        selectedCategories() {
            this.isSelected = this.selectedCategories.includes(this.id);
        },
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        }
    },
    mounted() {
        this.dropdownId = this.segmentId + '-Dropdown' + this.id ;
        this.checkboxId = this.segmentId + '-Checkbox' + this.id;
        if (this.isCustomSegment)
            this.dropdownId += this.$parent.id;
    },
    methods: {
        thisToSegment() {
            if (!this.isCustomSegment)
                this.$parent.loadSegment(this.id);
        },
        selectCategory() {
            if (this.editMode) {
                this.isSelected = this.selectedCategories.includes(this.id);

                if (this.isSelected)
                    this.$emit('unselect-category', this.id);
                else
                    this.$emit('select-category', this.id);
            }
        },
        splitRelation() {

        }
    }
});

var segmentComponent = Vue.component('segment-component', {
    props: {
        title: String,
        description: String,
        childCategoryIds: String,
        categoryId: String,
        editMode: Boolean,
    },

    data() {
        return {
            categories: [],
            id: null,
            cardsKey: null,
            isCustomSegment: true,
            visible: true,
            selectedCategories: [],
            currentChildCategoryIds: [],
            hover: false,
            showHover: false,
            addCategoryId: ""
        };
    },

    created() {
    },

    mounted() {
        this.id = "Segment-" + this.categoryId;
        if (this.childCategoryIds != null)
            this.currentChildCategoryIds = JSON.parse(this.childCategoryIds);
        var segment = {
            CategoryId: parseInt(this.categoryId),
            Title: this.title,
            ChildCategoryIds: this.categories
        }
        this.addCategoryId = "AddCategoryTo-" + this.id + "-Btn";
        this.$emit('new-segment', segment);
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
            this.categories = $("#" + this.id + " > .topic").map((idx, elem) => $(elem).attr("category-id")).get();
        },
        removeSegment() {
            this.$emit('remove-segment', parseInt(this.categoryId));
        },
        addCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            var self = this;
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#" + self.addCategoryId)
            }
            $('#AddCategoryModal').data('parent', parent).modal('show');        }
    },
});