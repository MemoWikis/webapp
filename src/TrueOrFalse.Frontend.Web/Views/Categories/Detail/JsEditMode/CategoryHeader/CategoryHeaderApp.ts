new Vue({
    name: 'CategoryHeader',
    el: '#CategoryHeader',
    data() {
        return {
            showMyWorld: false,
            isMounted: false,
        }
    },
    mounted() {
        this.isMounted = true;
    }
})