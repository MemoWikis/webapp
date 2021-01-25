interface Segment {
    title: String,
    description: String,
    categoryList: Array<Number>,
};


var segmentationComponent = Vue.component('segmentation-component', {
    props: {
        categoryId: Number,
        editMode: Boolean,
    },

    data() {
        return {
            baseCategoryList: [],
            customCategoryList: [] as Segment[],
            componentKey: 0,
            selectedCategoryId: null,
            isCustomSegment: false,
            hasCustomSegment: false,
        };
    },

    created() {
    },

    mounted() {
        this.hasCustomSegment = $('#CustomSegmentSection').html().length > 0;
    },

    watch: {
    },

    updated() {
    },

    methods: {
        forceRerender() {
            this.componentKey += 1;
        },
        loadSegment(id) {
            var currentElement = $("#CustomSegmentSection");
            var data = { CategoryId: id }
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetSegmentHtml',
                data: JSON.stringify(data),
                success: function (data) {
                    if (data) {
                        this.hasCustomSegment = true;
                        var inserted = currentElement.append(data.html);
                        var instance = new categoryCardComponent({
                            el: inserted.get(0)
                        });
                        this.$refs[id].destroy();
                        this.removeChild(this.$refs[id]);
                        this.forceRerender();
                    } else {
                    };
                },
            });
        }
    },
});