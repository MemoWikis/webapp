Vue.component('segment-component', {
    props: {
        name: String,
        description: String,
        baseCategoryList: Array,
    },

    data() {
        return {
            categories: [],
            id: null,
            canBeEdited: false,
        };
    },

    created() {
    },

    mounted() {
        this.id = "Segment-" + this._uid;
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
            var currentElement = $("#" + this.id + " > .topic");

            $.ajax({
                type: 'Get',
                contentType: "application/json",
                url: '/SegmentationController/GetCategoryCard',
                data: { categoryId: id },
                success: function (data) {
                    if (data) {
                        var inserted = $(data.newHtml).append(currentElement);
                        var instance = new categoryCardComponent({
                            el: inserted.get(0)
                        });
                    } else {

                    };
                },
            });
        }
    },
});