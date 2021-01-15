interface Segment {
    title: String,
    description: String,
    categoryList: Array<Number>,
};


var segmentationComponent = Vue.component('segmentation-component', {
    props: {
        categoryId: Number,
    },

    data() {
        return {
            baseCategoryList: [],
            customCategoryList: [] as Segment[],
            editMode: false,
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
    },
});