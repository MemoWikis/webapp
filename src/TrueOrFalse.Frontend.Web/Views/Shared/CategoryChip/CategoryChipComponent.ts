
Vue.component('categorychip-component', {
    props: ['category', 'index'],
    data() {
        return {
            showImage: false,
            hover: false,
        }
    },

    mounted() {
        if (!this.category.MiniImageUrl.includes('no-category-picture'))
            this.showImage = true;
    },

    methods: {
        removeCategory() {
            let data = {
                index: this.index,
                categoryId: this.category.Id
            }
            this.$emit('remove-category-chip', data);
        }
    }
})