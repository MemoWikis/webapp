
Vue.component('categorychip-component', {
    props: ['category'],
    data() {
        return {
            showImage: false,
        }
    },

    mounted() {
        if (!this.category.MiniImageUrl.includes('no-category-picture'))
            this.showImage = true;
    },

    methods: {
    }
})