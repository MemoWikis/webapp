new Vue({
    el: '#HeaderSearch',
    data() {
        return {
            showSearch: false,
            searchType: SearchType.All,
            windowWidht: 0,
        }
    },
    created() {
        this.windowWidth = window.innerWidth;
        if (this.windowWidth > 750)
            this.showSearch = true;
    },
    watch: {
        showSearch(val) {
            let logoContainer = $('#LogoContainer');
            let headerBodyContainer = $('#HeaderBodyContainer');
            let loginAndHelp = $('#loginAndHelp');

            if (val && this.windowWidth <= 750) {
                logoContainer.addClass('hidden-xs');
                headerBodyContainer.removeClass('col-xs-10');
                headerBodyContainer.addClass('col-xs-12');
                loginAndHelp.addClass('hidden-xs');
            } else if (!val && this.windowWidth <= 750) {
                logoContainer.removeClass('hidden-xs');
                headerBodyContainer.addClass('col-xs-10');
                headerBodyContainer.removeClass('col-xs-12');
                loginAndHelp.removeClass('hidden-xs');
            }
        }
    },
    methods: {
        openUrl(val) {
            location.href = val.Url;
        }
    }
})