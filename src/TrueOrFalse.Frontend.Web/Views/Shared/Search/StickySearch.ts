new Vue({
    el: '#StickySearch',
    data() {
        return {
            showSearch: false,
            searchType: SearchType.All,
            windowWidth: 0,
        }
    },
    created() {
        this.windowWidth = window.innerWidth;
    },
    watch: {
        showSearch(val) {
            if (val && this.windowWidth <= 750)
                $('#BreadCrumbContainer').addClass('search-is-open');
            else
                $('#BreadCrumbContainer').removeClass('search-is-open');
        }
    },
    methods: {
        openUrl(val) {
            location.href = val.Url;
        }
    }
})