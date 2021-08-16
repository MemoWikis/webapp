new Vue({
    el: '#StickySearch',
    data() {
        return {
            searchType: SearchType.All,
        }
    },
    created() {
    },
    watch: {
    },
    methods: {
        openUrl(val) {
            location.href = val.Url;
        }
    }
})