var categoryCardComponent = Vue.component('category-card-component', {
    props: {
        categoryId: String,
        editMode: Boolean,
        isCustomSegment: Boolean,
        selectedCategories: Array,
    },

    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null,
            id: parseInt(this.categoryId),
            isSelected: false,
        };
    },
    mounted() {
        this.dropdownId = 'Dropdown' + this.id;

        if (this.isCustomSegment)
            this.dropdownId += this.$parent.id;
    },
    methods: {
        thisToSegment() {
            this.$parent.loadSegment(this.id);
        },
        selectCategory() {
            if (this.isEditMode) {
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
        categoryId: Number,
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
            currentChildCategoryIds: []
        };
    },

    created() {
    },

    mounted() {
        var self = this;
        self.id = "Segment-" + self.categoryId;
        self.currentChildCategoryIds = JSON.parse(self.childCategoryIds);
        var segment = {
            CategoryId: self.categoryId,
            Title: self.title,
            ChildCategoryIds: self.categories
        }
        this.$emit('new-segment', segment);
        this.$on('select-category',
            (id) => {
                if (this.selectedCategories.includes(id))
                    return;
                else this.selectedCategories.push(id);
            });
        this.$on('unselect-category',
            (id) => {
                if (this.selectedCategories.includes(id)) {
                    var index = this.selectedCategories.indexOf(id);
                    this.selectedCategories.splice(index, 1);
                }
            });
    },

    watch: {
    },

    updated() {
    },

    methods: {
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
            this.$emit('remove-segment', this.categoryId);
        },
    },
});