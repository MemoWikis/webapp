Vue.component('segment-component', {
    props: {
        name: String,
        description: String,
        baseCategoryList: Array,
    },

    data() {
        return {
            categories: []
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
        getChildrenForCustomCategories() {

        }
    },
});