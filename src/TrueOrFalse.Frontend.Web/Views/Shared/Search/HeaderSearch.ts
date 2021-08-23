new Vue({
    el: '#HeaderSearch',
    data() {
        return {
            searchType: SearchType.All,
        }
    },
    methods: {
        openUrl(val) {
            location.href = val.Url;
        }
    }
});

new Vue({
    el: '#SmallHeaderSearch',
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