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
        };
    },

    created() {
    },

    mounted() {
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
                        currentElement.append(data.html);
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