var categoryCardComponent = Vue.component('category-card-component', {
    props: {
        categoryId: String,
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
        };
    },

    created() {
    },

    mounted() {
        var self = this;
        self.id = "Segment-" + self.categoryId;
        if (self.childCategoryIds != null)
            self.currentChildCategoryIds = JSON.parse(self.childCategoryIds);
        var segment = {
            CategoryId: parseInt(self.categoryId),
            Title: self.title,
            ChildCategoryIds: self.categories
        }
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
        loadCategoryCard(id) {
            var self = this;
            var currentElement = $("#" + this.id + " > .topicNavigation");

            $.ajax({
                type: 'Get',
                contentType: "application/int",
                url: '/Segmentation/GetCategoryCard',
                data: id,
                success: function (data) {
                    if (data) {
                        currentElement.append(data.html);
                    } else {

                    };
                },
            });
        },
        removeSegment() {
            this.$emit('remove-segment', parseInt(this.categoryId));
        },
    },
});