Vue.component('category-card-component', {
    props: {
        categoryId: String,
        editMode: Boolean,
        isCustomSegment: Boolean,
    },

    methods: {
        thisToSegment() {
            var id = parseInt(this.categoryId);
            this.$parent.loadSegment(id);
        },
    }
});

Vue.component('segment-component', {
    props: {
        name: String,
        description: String,
        baseCategoryList: Array,
        editMode: Boolean,
    },

    data() {
        return {
            categories: [],
            id: null,
            cardsKey: null,
            isCustomSegment: true,
        };
    },

    created() {
    },

    mounted() {
        var uid = this._uid;
        this.id = "Segment-" + uid;
        this.cardsKey = uid;
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
            var currentElement = $("#" + this.id + " > .topic");

            $.ajax({
                type: 'Get',
                contentType: "application/int",
                url: '/Segmentation/GetCategoryCard',
                data: id,
                success: function (data) {
                    if (data) {
                        currentElement.append(data.html);
                        this.forceRerender();
                    } else {

                    };
                },
            });
        }
    },
});