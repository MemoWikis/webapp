Vue.component('lazy-component',
    {
        template: '#lazy-component',

        data() {
            return {
                shouldRender: false,
            }
        },
        mounted() {
            this.$nextTick(() => this.shouldRender = true);
        },
    });