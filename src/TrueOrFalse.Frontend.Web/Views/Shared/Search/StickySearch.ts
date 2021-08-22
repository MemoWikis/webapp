new Vue({
    el: '#StickySearch',
    data() {
        return {
            showSearch: false,
            searchType: SearchType.All,
        }
    },
    methods: {
        openUrl(val) {
            location.href = val.Url;
        }
    }
})