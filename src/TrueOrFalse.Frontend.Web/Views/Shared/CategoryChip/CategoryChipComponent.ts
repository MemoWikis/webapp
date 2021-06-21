
Vue.component('categorychip-component', {
    props: ['category', 'index'],
    data() {
        return {
            showImage: false,
            hover: false,
            name: '',
        }
    },

    mounted() {
        if (!this.category.MiniImageUrl.includes('no-category-picture'))
            this.showImage = true;
        this.name = this.category.Name.length > 30 ? this.category.Name.substring(0, 26) + ' ...' : this.category.Name;
        $('.show-tooltip').tooltip();
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