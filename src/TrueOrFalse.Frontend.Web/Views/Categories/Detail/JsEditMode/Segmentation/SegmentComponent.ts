var categoryCardComponent = Vue.component('category-card-component', {
    props: {
        categoryId: String,
        editMode: Boolean,
        isCustomSegment: Boolean,
    },

    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null,
        };
    },
    mounted() {
        this.dropdownId = 'Dropdown' + this.categoryId;

        if (this.isCustomSegment)
            this.dropdownId += this.$parent.id;
    },
    methods: {
        thisToSegment() {
            var id = parseInt(this.categoryId);
            this.$parent.loadSegment(id);
        },
    }
});

var segmentComponent = Vue.component('segment-component', {
    props: {
        title: String,
        description: String,
        ChildCategoryIds: String,
        editMode: Boolean,
    },

    data() {
        return {
            categories: [],
            id: null,
            cardsKey: null,
            isCustomSegment: true,
            visible: true,
        };
    },

    created() {
    },

    mounted() {
        var self = this;
        var uid = this._uid;
        self.id = "Segment-" + uid;
        self.cardsKey = uid;
        var segment = {
            CategoryId: self.id,
            Title: self.title,
            ChildCategoryIds: self.categories
        }
        this.$emit('new-segment', segment);
    },

    watch: {
    },

    updated() {
    },

    methods: {
        forceRerender() {
            this.cardsKey += 1;
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
                        self.forceRerender();
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